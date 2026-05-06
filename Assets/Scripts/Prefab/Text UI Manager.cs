using UnityEngine;

public class TextUIManager : MonoBehaviour
{
    [SerializeField]
    private InteractableText interactableText;

    void Awake()
    {
        if (interactableText == null)
        {
            throw new System.Exception("interactableText is not assigned in this script");
        }
    }

    void Start()
    {
        GameText gameText = interactableText.GetGameText();
        
    }
}
