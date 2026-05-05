/*
    Created by: John
    Description: Handles the informative in-game text parts of the game
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableText : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToHighlight;
    public string interactibleName; // May be removed if unused
    [SerializeField]
    private List<GameText> texts;

    [Header("Hidden Parameters")]
    [SerializeField]
    private int currentTextElementNumber = 0;

    [SerializeField]
    private bool textStart = false;
    [SerializeField]
    private bool taskActive = false;

    [Header("Events")]
    [Space(5)]
    // Events
    public UnityEvent onTextsStart;
    public UnityEvent onTextChange;
    public UnityEvent onTextsEnd;

    void Awake()
    {

        // Sets the object to be highlighted if no set object
        if (objectToHighlight == null)
        {
            objectToHighlight = gameObject;
        }

        StartCoroutine(WaitForGameManagerInstance());
    }

    private IEnumerator WaitForGameManagerInstance()
    {
        // Wait for the game manager to load before adding this script
        yield return new WaitUntil(() => GameManager.instance != null);
        GameManager.instance.onGameReset.AddListener(ResetProgress);

        // Adds this script to the game manager for ordered interaction
        bool success = GameManager.instance.AddInteractibleText(this);

        if (success)
        {
            Debug.Log("Successfully added interactible text to game manager");
        }
    }

    private void ResetProgress()
    {
        currentTextElementNumber = 0;
        textStart = false;
        taskActive = false;
    }
    
    public GameText GetTextObject()
    {
        if (!textStart) {onTextsStart.Invoke(); currentTextElementNumber = 0; textStart = true;}
        return texts[currentTextElementNumber];
    }

    public void SelectOption(int option)
    {
        GetNextTextObject(option);
    }

    private GameText GetNextTextObject(int option = -1)
    {

        if (taskActive == true)
        {
            Debug.LogWarning("There is a task currently active");
            return texts[currentTextElementNumber];
        }

        GameText currentText = texts[currentTextElementNumber];
        int nextTextElementNumber = currentTextElementNumber + 1;

        if (currentText.options.Count == -1)
        {
            if (currentText.jumpTo > -1)
            {
                nextTextElementNumber = currentText.jumpTo;
            }
            else
            {
                nextTextElementNumber++;

                if (nextTextElementNumber == texts.Count)
                {
                    Debug.Log("Finished text block");
                    onTextsEnd.Invoke();
                    textStart = false;
                }
            }
        }
        else if (0 <= option && option < currentText.options.Count)
        {
            TextOptions chosenOption = currentText.options[option];
            
            if (chosenOption.jumpTo < 0) 
            {
                Debug.LogError($"Invalid text number to jump to\nText Element Number: {currentTextElementNumber}\nOption given: {option}\nOption to jump to: {currentText.options[option].jumpTo}");
            }
            else
            {
                nextTextElementNumber = chosenOption.jumpTo;
            }
        }
        else
        {
            Debug.LogError($"Invalid option given\nText Element Number: {currentTextElementNumber}\nTotal option amount: {currentText.options.Count}\nOption given: {option}");
        }

        if (currentTextElementNumber != nextTextElementNumber)
        {
            onTextChange.Invoke();
            currentTextElementNumber = nextTextElementNumber;

            if (texts[currentTextElementNumber].taskToComplete != null)
            {
                taskActive = true;
                texts[currentTextElementNumber].taskToComplete.onTaskComplete.AddListener(OnTaskComplete);
            }
        }

        return texts[currentTextElementNumber];
    }

    private void OnTaskComplete()
    {
        texts[currentTextElementNumber].taskToComplete.onTaskComplete.RemoveListener(OnTaskComplete);
        GetNextTextObject(-1);
    }

}

[System.Serializable]
public class GameText
{
    public string text;
    public int jumpTo = -1;
    public InteractableTask taskToComplete;
    public List<TextOptions> options;
}

[System.Serializable]
public class TextOptions
{
    public bool isAButton = false;
    public string text;
    public int jumpTo = -1;
    public int pointsToAward = 0;
}