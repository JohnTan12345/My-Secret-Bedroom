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
    private GameObject objectHighlighted;
    public string interactibleName; // May be removed if unused
    [Tooltip("This is the text used if there are no options")]
    public string continueText = "Continue";
    [SerializeField]
    private List<GameText> texts;

    [Header("Hidden Parameters")]
    [SerializeField]
    private int currentTextElementNumber = 0;

    [SerializeField]
    private bool textStart = false;
    [SerializeField]
    private bool taskActive = false;
    [SerializeField]
    private bool listenerAdded = false;
    [SerializeField]
    private bool debuggingEnabled = false;

    [Header("Events")]
    [Space(5)]
    // Events
    public UnityEvent onTextsStart; // Fires when the texts starts
    public UnityEvent onTextChange; // Fires when the current text changes
    public UnityEvent onTextsEnd; // Fires when there is no more text after the current text

    void Awake()
    {
        StartCoroutine(WaitForGameManagerInstance());
    }

    private IEnumerator WaitForGameManagerInstance()
    {
        // Wait for the game manager to load before adding this script
        yield return new WaitUntil(() => GameManager.instance != null);
        GameManager.instance.onGameReset.AddListener(ResetTextProgress);

        // Adds this script to the game manager for ordered interaction
        GameManager.instance.AddInteractableText(this);
    }
    
    public GameText GetGameText()
    {
        if (debuggingEnabled)
        {
            Debug.Log("Getting Game Text");
        }
        if (!textStart) {onTextsStart.Invoke(); currentTextElementNumber = 0; textStart = true;}
        if (!taskActive && texts[currentTextElementNumber].taskToComplete != null) {taskActive = true; AddOnCompleteListener();}
        return texts[currentTextElementNumber];
    }

// Text related Functions

    // Selects an option (defaults to -1 if none given)
    public void SelectOption(int option = -1)
    {
        GetNextTextObject(option);
    }

    // Get next text based on option
    private GameText GetNextTextObject(int option)
    {

        if (taskActive == true) // Return the current text if there is a task active
        {
            Debug.LogWarning("There is a task currently active");
            return texts[currentTextElementNumber];
        }

        GameText currentText = texts[currentTextElementNumber];
        int nextTextElementNumber = currentTextElementNumber + 1;

        if (currentText.options.Count == 0) // If there are no options and the current text jump to is within 0 and above
        {
            if (currentText.jumpTo > -1)
            {
                nextTextElementNumber = currentText.jumpTo;
            }
            
        }
        else if (0 <= option && option < currentText.options.Count) // If there are options and the given option is within the range
        {
            TextOptions chosenOption = currentText.options[option]; // Get the option object
            
            if (chosenOption.jumpTo < -1) // Option validator
            {
                throw new System.Exception($"Invalid text number to jump to\nText Element Number: {currentTextElementNumber}\nOption given: {option}\nOption to jump to: {currentText.options[option].jumpTo}");
            }
            else if (chosenOption.jumpTo > -1) // If the option has a specific text to jump to
            {
                nextTextElementNumber = chosenOption.jumpTo;
            }

            GameManager.instance.AddPoints(chosenOption.pointsToAward);
        }
        else // If option is over/under the allowed range
        {
            throw new System.Exception($"Invalid option given\nText Element Number: {currentTextElementNumber}\nTotal option amount: {currentText.options.Count}\nOption given: {option}");
        }

        if (currentTextElementNumber != nextTextElementNumber)
        {
            if (nextTextElementNumber < texts.Count)
            {
                // Fire the event after changing the current text element
                currentTextElementNumber = nextTextElementNumber;
                onTextChange.Invoke();

                if (texts[currentTextElementNumber].taskToComplete != null) // Checks if the current text has a task to complete
                {
                    taskActive = true;
                    texts[currentTextElementNumber].taskToComplete.onTaskStart.Invoke();
                    AddOnCompleteListener();
                    if (debuggingEnabled)
                    {
                        Debug.Log("Added listener to onTaskComplete");
                    }
                }
            }
            else
            {
                // Fire the event when the final text for a branch is reached
                onTextsEnd.Invoke();
                textStart = false;

                if (debuggingEnabled)
                {
                    Debug.Log($"Texts has ended at text element: {currentTextElementNumber}\nOptions available:{texts[currentTextElementNumber].options.Count > 0}{(texts[currentTextElementNumber].options.Count > 0 ? $"Ended at option {option} jumping to text element {texts[currentTextElementNumber].options[option].jumpTo}":"")}");
                }
            }
        }

        return texts[currentTextElementNumber];
    }

    private void AddOnCompleteListener()
    {
        if (!listenerAdded)
        {
            texts[currentTextElementNumber].taskToComplete.onTaskComplete.AddListener(OnTaskComplete);
            listenerAdded = true;
        }
    }

    private void RemoveOnCompleteListener()
    {
        texts[currentTextElementNumber].taskToComplete.onTaskComplete.RemoveListener(OnTaskComplete);
        listenerAdded = false;
    }

    private void ResetTextProgress() // Resets the text
    {
        if (texts.Count == 0) {return;}
        if (texts[currentTextElementNumber].taskToComplete != null) {RemoveOnCompleteListener();}
        currentTextElementNumber = 0;
        onTextChange.Invoke();
        textStart = false;
        taskActive = false;
    }

    private void OnTaskComplete() // Wrapper for when the task is complete
    {
        Debug.Log($"{currentTextElementNumber}\n{texts[currentTextElementNumber].taskToComplete}");
        RemoveOnCompleteListener();
        taskActive = false;
        GetNextTextObject(-1);
    }

}

[System.Serializable]
public class GameText
{
    public string text;
    [Tooltip("To move on to the next text, set to -1. Else set it to the next text index")]
    public int jumpTo = -1;
    public InteractableTask taskToComplete;
    public List<TextOptions> options;
}

[System.Serializable]
public class TextOptions
{
    public bool isAButton = false;
    public string text;
    [Tooltip("To move on to the next text, set to -1. Else set it to the next text index")]
    public int jumpTo = -1;
    public int pointsToAward = 0;
}