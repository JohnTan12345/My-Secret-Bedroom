/*
    Created by: John
    Description: Toggles the text UI based on whether the player is looking at the UI / interactable or not.
*/

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LookUIToggle : MonoBehaviour
{
    [SerializeField]
    [Tooltip("An area where the player can interact with the object. Can be left null")]
    private EnterArea objectInteractionArea;
    [SerializeField]
    [Tooltip("If the object can be grabbed and you want the UI to appear only when the object is grabbed after first grab. Can be left null")]
    private XRGrabInteractable grabInteractable;
    [SerializeField]
    private bool disableInteractionAreaOnGrab = false;
    [SerializeField]
    private GameObject objectLookArea;
    [SerializeField]
    private GameObject textUILookArea;
    [SerializeField]
    [Tooltip("If empty, it will use textUILookArea")]
    private GameObject textUIObject;

    [Header("Hidden Parameters")]
    [SerializeField]
    private bool lookingAtObject = false;
    [SerializeField]
    private bool lookingAtUI = false;
    [SerializeField]
    private bool areaInteractionEnabled = false;
    [SerializeField]
    private bool areaInteractionActive = false;
    [SerializeField]
    private bool playerInArea = false;
    [SerializeField]
    private bool playerGrabbing = false;
    [SerializeField]
    private bool playerGrabbedOnce = false;
    [SerializeField]
    private bool playerLookedInArea = false;

    // Variable setup and adding listeners
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(ResetValues);

        // Assign look area for the object
        LookArea _objectLookArea = objectLookArea.GetComponent<LookArea>();
        _objectLookArea.onPlayerLook.AddListener(() => LookingAtObject(true));
        _objectLookArea.onPlayerAway.AddListener(() => LookingAtObject(false));

        // Assign look area for the text UI
        LookArea _textUILookArea = textUILookArea.GetComponent<LookArea>();
        _textUILookArea.onPlayerLook.AddListener(() => LookingAtUI(true));
        _textUILookArea.onPlayerAway.AddListener(() => LookingAtUI(false));

        // If the object should only be interactable within an area the first time
        if (objectInteractionArea != null)
        {
            areaInteractionEnabled = true;
            areaInteractionActive = true;
            objectInteractionArea.ObjectEnterArea.AddListener(() => PlayerInArea(true));
            objectInteractionArea.ObjectExitArea.AddListener(() => PlayerInArea(false));
        }

        // If a grab XR interactable script is assigned
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener((SelectEnterEventArgs) => PlayerGrabbing(true));
            grabInteractable.selectExited.AddListener((SelectExitEventArgs) => PlayerGrabbing(false));
        }

        // Assign the text UI gameObject to the Look Area if theres no given gameObject (UI)
        if (textUIObject == null)
        {
            textUIObject = textUILookArea;
        }
    }

    // Update the UI according to whether the player is looing at the interactable object
    public void LookingAtObject(bool val)
    {
        if (val == lookingAtObject)
        {
            return;
        }

        lookingAtObject = val;
        UpdateUI();
    }

    // Update the UI according to whether the player is looing at the text UI
    public void LookingAtUI(bool val)
    {
        if (val == lookingAtUI)
        {
            return;
        }

        lookingAtUI = val;
        UpdateUI();
    }

    // Update the UI according to whether the player is within the interaction area
    public void PlayerInArea(bool val)
    {
        if (val == playerInArea)
        {
            return;
        }

        playerInArea = val;
        UpdateUI();
    }

    // Enable/Disable the enter area object
    public void SetActiveAreaInteraction(bool val)
    {
        if (areaInteractionEnabled)
        {
            areaInteractionActive = val;
        }
    }

    // Disable interaction area while being grabbed (if set). Also remembers that the player grabbed it the first time
    private void PlayerGrabbing(bool val)
    {
        if (disableInteractionAreaOnGrab)
        {
            areaInteractionActive = false;
        }
        playerGrabbedOnce = true;
        playerGrabbing = val;
    }

    // Enables/Disables the UI by checking if the player is looking at the UI/GameObject/Still within interaction area
    private void UpdateUI()
    {
        bool _playerInAreaAndobjectInteractionAreaSet = areaInteractionActive && playerInArea || !areaInteractionActive;
        bool _playerGrabbing = playerGrabbedOnce && playerGrabbing || !playerGrabbedOnce;

        bool _textActiveAndLooking = textUIObject.activeSelf && (lookingAtObject || lookingAtUI);
        bool _textNotActiveAndLookingAtObject = !textUIObject.activeSelf && lookingAtObject;

        // Check if the player is within the interaction area and is looking
        if (areaInteractionActive)
        {
            if ((_textActiveAndLooking || _textNotActiveAndLookingAtObject) && _playerInAreaAndobjectInteractionAreaSet || playerLookedInArea && _playerInAreaAndobjectInteractionAreaSet)
            {
                playerLookedInArea = true;
            }
            else 
            {
                playerLookedInArea = false;
            }
        }
        
        // Enable the UI if the player is within interaction area (if set), looking at the object and grabbing it (if grabbed at least once). Else disable the UI
        if ((_playerGrabbing && _playerInAreaAndobjectInteractionAreaSet && (_textActiveAndLooking || _textNotActiveAndLookingAtObject)) || playerLookedInArea && areaInteractionActive)
        {
            textUIObject.SetActive(true);
        }
        else
        {
            textUIObject.SetActive(false);
        }
    }

    // Resets values to original
    private void ResetValues()
    {
        if (areaInteractionEnabled)
        {
            areaInteractionActive = true;
        }
        playerGrabbing = false;
        playerGrabbedOnce = false;
    }
}
