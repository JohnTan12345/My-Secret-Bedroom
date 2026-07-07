/*
    Created by: John
    Description: Text UI functions
*/

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextUIManager : MonoBehaviour
{
    [SerializeField]
    private InteractableText interactableText;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private Transform optionUI_Parent;
    [SerializeField]
    private GameObject optionUI_Prefab;
    [SerializeField]
    private GameObject referenceGameObjectForActiveCheck;

    private bool listenerAdded = false;

    [HideInInspector]
    public UnityEvent<int> buttonClickedEvent;

    [Header("Hidden Parameters")]
    [SerializeField]
    private bool startOnReset = false;
    [SerializeField]
    private bool textStarted = false;
    [SerializeField]
    private bool canLoopBack = true;
    [SerializeField]
    private bool debuggingEnabled = false;
    void Awake()
    {
        if (interactableText == null)
        {
            gameObject.TryGetComponent(out interactableText);

            if (interactableText == null)
            {
                throw new System.Exception("interactableText is not assigned in this script");
            }
        }
    }

    void Start()
    {   
        GameManager.instance.onGameReset.AddListener(Reset);
        interactableText.onTextChange.AddListener(RefreshText);
        interactableText.onTextsEnd.AddListener(Loopback);
    }

    void OnEnable()
    {
        if (!textStarted)
        {
            textStarted = true;
            RefreshText();
        }
        interactableText.StartHeadDetection();
    }

    void OnDisable()
    {
        interactableText.StopHeadDetection("Text UI Disabled");
    }

    private void Reset()
    {
        if (debuggingEnabled)
        {
            Debug.Log($"Resetting Text UI for {interactableText.gameObject.name}");
        }
        textStarted = false;
        if (!canLoopBack)
        {
            interactableText.onTextsEnd.AddListener(Loopback);
            canLoopBack = true;
        }
    }

    private void Loopback()
    {
        if (debuggingEnabled)
        {
            Debug.Log("Looping");
        }
        if (!canLoopBack)
        {
            return;
        }
        interactableText.ResetTextProgress();
        textStarted = false;
        RefreshText();
    }

    private void RefreshText() // Refreshes the UI everytime the current text changes
    {
        if (debuggingEnabled)
        {
            Debug.Log($"Refreshing Text UI for {interactableText.gameObject.name}");
        }
        GameText gameText = interactableText.GetGameText();

        for (int i = optionUI_Parent.childCount; i > 0; i--) // Remove the option UI
        {
            int childIndex = i - 1;
            GameObject optionUI_Child = optionUI_Parent.GetChild(childIndex).gameObject;
            optionUI_Child.GetComponent<TextOptionUI>().button.onClick.RemoveAllListeners();
            Destroy(optionUI_Child);
        }

        infoText.text = gameText.text;

        if (gameText.taskToComplete == null && interactableText.TextsCount > 1) // Check if the current text does not contain a task
        {

            if (!listenerAdded) // Add a new listener to the event if it has not been added yet
            {
                buttonClickedEvent.AddListener(interactableText.SelectOption);
                listenerAdded = true;
            }
            

            if (gameText.options.Count > 0) // Check if there are available options
            {
                for (int j = 0; j < gameText.options.Count; j++) // Clone a new option UI for each option
                {
                    TextOptions option = gameText.options[j];
                    GameObject optionUI_Clone = Instantiate(optionUI_Prefab, optionUI_Parent);
                    optionUI_Clone.name = $"OptionUI {j}";

                    TextOptionUI textOptionUI = optionUI_Clone.GetComponent<TextOptionUI>();
                    int optionNum = j;
                    if (gameText.headMovementOption)
                    {
                        textOptionUI.button.interactable = false;
                    }
                    else
                    {
                        textOptionUI.button.onClick.AddListener( () => buttonClickedEvent.Invoke(optionNum) );
                    }
                    textOptionUI.text.text = gameText.headMovementOption ? option.text + $" ({(j % 2 == 0  ? "nod":"shake")})": option.text;
                }
            }
            else
            {
                // Clone a new option UI that allows the player to go to the next text
                GameObject optionUI_Clone = Instantiate(optionUI_Prefab, optionUI_Parent);

                TextOptionUI textOptionUI = optionUI_Clone.GetComponent<TextOptionUI>();
                textOptionUI.button.onClick.AddListener( () => buttonClickedEvent.Invoke(-1) );
                textOptionUI.text.text = interactableText.continueText;
            }
        }
        else // If the task contains a task
        {
            buttonClickedEvent.RemoveListener(interactableText.SelectOption);
            listenerAdded = false;
            canLoopBack = false;
            interactableText.onTextsEnd.RemoveListener(Loopback);
        }
    }
}
