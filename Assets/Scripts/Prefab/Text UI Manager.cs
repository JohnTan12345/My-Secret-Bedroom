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

        buttonClickedEvent.AddListener(interactableText.SelectOption);
    }

    void Start()
    {   
        interactableText.onTextChange.AddListener(RefreshText);
        RefreshText();
    }

    private void RefreshText()
    {
        GameText gameText = interactableText.GetGameText();

        int i = optionUI_Parent.childCount;
        while (i > 0)
        {
            int childIndex = i - 1;
            optionUI_Parent.GetChild(childIndex).GetComponent<TextOptionUI>().button.onClick.RemoveAllListeners();
            Destroy(optionUI_Parent.GetChild(childIndex).gameObject);
            i--;
        }

        infoText.text = gameText.text;

        if (gameText.options.Count > 0)
        {
            for (int j = 0; j < gameText.options.Count; j++)
            {
                TextOptions option = gameText.options[j];
                GameObject optionUI_Clone = Instantiate(optionUI_Prefab);
                optionUI_Clone.transform.parent = optionUI_Parent;

                TextOptionUI textOptionUI = optionUI_Clone.GetComponent<TextOptionUI>();
                textOptionUI.optionNum = j;
                textOptionUI.button.onClick.AddListener( () => buttonClickedEvent.Invoke(textOptionUI.optionNum) );
                textOptionUI.text.text = option.text;
            }
        }
        else
        {
            GameObject optionUI_Clone = Instantiate(optionUI_Prefab);
            optionUI_Clone.transform.parent = optionUI_Parent;

            TextOptionUI textOptionUI = optionUI_Clone.GetComponent<TextOptionUI>();
            textOptionUI.button.onClick.AddListener( () => buttonClickedEvent.Invoke(-1) );
            textOptionUI.text.text = interactableText.continueText;
        }
    }
}
