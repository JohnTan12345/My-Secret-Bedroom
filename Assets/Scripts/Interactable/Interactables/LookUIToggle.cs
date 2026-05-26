using UnityEngine;
using UnityEngine.Events;
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

    void Start()
    {
        GameManager.instance.onGameReset.AddListener(ResetValues);

        LookArea _objectLookArea = objectLookArea.GetComponent<LookArea>();
        _objectLookArea.onPlayerLook.AddListener(() => LookingAtObject(true));
        _objectLookArea.onPlayerAway.AddListener(() => LookingAtObject(false));

        LookArea _textUILookArea = textUILookArea.GetComponent<LookArea>();
        _textUILookArea.onPlayerLook.AddListener(() => LookingAtUI(true));
        _textUILookArea.onPlayerAway.AddListener(() => LookingAtUI(false));

        if (objectInteractionArea != null)
        {
            areaInteractionEnabled = true;
            areaInteractionActive = true;
            objectInteractionArea.ObjectEnterArea.AddListener(() => PlayerInArea(true));
            objectInteractionArea.ObjectExitArea.AddListener(() => PlayerInArea(false));
        }

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener((SelectEnterEventArgs) => PlayerGrabbing(true));
            grabInteractable.selectExited.AddListener((SelectExitEventArgs) => PlayerGrabbing(false));
        }

        if (textUIObject == null)
        {
            textUIObject = textUILookArea;
        }
    }

    public void LookingAtObject(bool val)
    {
        if (val == lookingAtObject)
        {
            return;
        }

        lookingAtObject = val;
        UpdateUI();
    }

    public void LookingAtUI(bool val)
    {
        if (val == lookingAtUI)
        {
            return;
        }

        lookingAtUI = val;
        UpdateUI();
    }

    public void PlayerInArea(bool val)
    {
        if (val == playerInArea)
        {
            return;
        }

        playerInArea = val;
        UpdateUI();
    }

    public void SetActiveAreaInteraction(bool val)
    {
        if (areaInteractionEnabled)
        {
            areaInteractionActive = val;
        }
    }

    private void PlayerGrabbing(bool val)
    {
        if (disableInteractionAreaOnGrab)
        {
            areaInteractionActive = false;
        }
        playerGrabbedOnce = true;
        playerGrabbing = val;
    }

    private void UpdateUI()
    {
        bool _playerInAreaAndobjectInteractionAreaSet = areaInteractionActive && playerInArea || !areaInteractionActive;
        bool _playerGrabbing = playerGrabbedOnce && playerGrabbing || !playerGrabbedOnce;

        bool _textActiveAndLooking = textUIObject.activeSelf && (lookingAtObject || lookingAtUI);
        bool _textNotActiveAndLookingAtObject = !textUIObject.activeSelf && lookingAtObject;

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
        

        if ((_playerGrabbing && _playerInAreaAndobjectInteractionAreaSet && (_textActiveAndLooking || _textNotActiveAndLookingAtObject)) || playerLookedInArea && areaInteractionActive)
        {
            textUIObject.SetActive(true);
        }
        else
        {
            textUIObject.SetActive (false);
        }
    }

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
