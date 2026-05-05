using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance {get; private set;}
    [SerializeField]
    [Tooltip("You can set an ordered list of interactible texts. Any interactible texts that is not added here will be automatically added with no order")]
    private List<InteractableText> interactibleTexts = new List<InteractableText>();

    void Awake()
    {
        if (instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    void StartGame()
    {
        
    }

    private void EndGame()
    {
        
    }

    public bool AddInteractibleText(InteractableText interactibleText)
    {
        if (interactibleTexts.Contains(interactibleText))
        {
            Debug.Log($"{interactibleText} is already inside {instance}");
            return false;
        }

        interactibleTexts.Add(interactibleText);
        return true;
    }
}
