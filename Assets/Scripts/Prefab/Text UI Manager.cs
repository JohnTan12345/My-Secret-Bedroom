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

    private bool listenerAdded = false;

    [HideInInspector]
    public UnityEvent<int> buttonClickedEvent;

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
        interactableText.onTextChange.AddListener(RefreshText);
        RefreshText();
    }

    private void RefreshText()
    {
        GameText gameText = interactableText.GetGameText();

        for (int i = optionUI_Parent.childCount; i > 0; i--)
        {
            int childIndex = i - 1;
            GameObject optionUI_Child = optionUI_Parent.GetChild(childIndex).gameObject;
            optionUI_Child.GetComponent<TextOptionUI>().button.onClick.RemoveAllListeners();
            Destroy(optionUI_Child);
        }

        infoText.text = gameText.text;

        if (gameText.taskToComplete == null)
        {

            if (!listenerAdded) 
            {
                buttonClickedEvent.AddListener(interactableText.SelectOption); listenerAdded = true;
            }
            

            if (gameText.options.Count > 0)
            {
                for (int j = 0; j < gameText.options.Count; j++)
                {
                    TextOptions option = gameText.options[j];
                    GameObject optionUI_Clone = Instantiate(optionUI_Prefab, optionUI_Parent);
                    optionUI_Clone.name = $"OptionUI {j}";

                    TextOptionUI textOptionUI = optionUI_Clone.GetComponent<TextOptionUI>();
                    int optionNum = j;
                    textOptionUI.button.onClick.AddListener( () => buttonClickedEvent.Invoke(optionNum) );
                    textOptionUI.text.text = option.text;
                }
            }
            else
            {
                GameObject optionUI_Clone = Instantiate(optionUI_Prefab, optionUI_Parent);

                TextOptionUI textOptionUI = optionUI_Clone.GetComponent<TextOptionUI>();
                textOptionUI.button.onClick.AddListener( () => buttonClickedEvent.Invoke(-1) );
                textOptionUI.text.text = interactableText.continueText;
            }
        }
        else
        {
            buttonClickedEvent.RemoveListener(interactableText.SelectOption);
            listenerAdded = false;
        }
    }
}
