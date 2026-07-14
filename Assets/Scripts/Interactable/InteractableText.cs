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
    public int TextsCount {get {return texts.Count;}}

    [Header("Hidden Parameters")]
    [SerializeField]
    private int currentTextElementNumber = 0;

    [SerializeField]
    private bool textStart = false;
    [SerializeField]
    private bool taskActive = false;
    [SerializeField]
    private bool taskListenerAdded = false;
    [SerializeField]
    private bool headDetectionListenerAdded = false;
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
    }
    
    public GameText GetGameText()
    {
        if (debuggingEnabled)
        {
            Debug.Log("Getting Game Text");
        }
        if (!textStart) {onTextsStart.Invoke(); currentTextElementNumber = 0; textStart = true;}
        if (!taskActive && texts[currentTextElementNumber].taskToComplete != null) {taskActive = true; SetTaskCompleteListener(true); texts[currentTextElementNumber].taskToComplete.onTaskStart.Invoke();}
        if (!headDetectionListenerAdded && texts[currentTextElementNumber].headMovementOption) {SetHeadDetectionCompleteListener(true);}

        return texts[currentTextElementNumber];
    }

    public void StopHeadDetection(string msg)
    {
        if (!headDetectionListenerAdded)
        {
            if (debuggingEnabled)
            {
                Debug.LogWarning("No active head detection found");
            }
            return;
        }

        SetHeadDetectionCompleteListener(false, msg);
    }

    public void StartHeadDetection()
    {
        if (headDetectionListenerAdded)
        {
            if (debuggingEnabled)
            {
                Debug.LogWarning("There is an active head detection");
            }
            return;
        }
        else if (!texts[currentTextElementNumber].headMovementOption)
        {
            if (debuggingEnabled)
            {
                Debug.LogWarning("Current element has head detection disabled");
            }
            return;
        }
        SetHeadDetectionCompleteListener(true);
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
                    SetTaskCompleteListener(true);
                    if (debuggingEnabled)
                    {
                        Debug.Log("Added listener to onTaskComplete");
                    }
                }
                else if (texts[currentTextElementNumber].headMovementOption)
                {
                    // Start head detection
                    if (!headDetectionListenerAdded)
                    {
                        SetHeadDetectionCompleteListener(true);
                    }

                }

                
            }
            else // In case the text ends with a task / head detection
            {
                // Fire the event when the final text for a branch is reached
                onTextsEnd.Invoke();
                textStart = false;
                if (debuggingEnabled)
                {
                    Debug.Log($"Texts has ended at text element: {currentTextElementNumber}");
                    Debug.Log($"Options available: {texts[currentTextElementNumber].options.Count > 0}");
                    //Debug.Log($"{(texts[currentTextElementNumber].options.Count > 0 ? $"Ended at option {option} jumping to text element {texts[currentTextElementNumber].options[option].jumpTo}":"")}");
                }
            }
        }

        return texts[currentTextElementNumber];
    }

// Listener adding / removing
    // Task completion listener
    private void SetTaskCompleteListener(bool val)
    {
        if (val && !taskListenerAdded)
        {
            texts[currentTextElementNumber].taskToComplete.onTaskComplete.AddListener(OnTaskComplete);
            taskListenerAdded = val;
        }
        else if (!val)
        {
            texts[currentTextElementNumber].taskToComplete.onTaskComplete.RemoveListener(OnTaskComplete);
            taskListenerAdded = val;
        }
    }

    // Head detection complete listener
    private void SetHeadDetectionCompleteListener(bool val, string msg = "stop called")
    {
        if (val && !headDetectionListenerAdded)
        {
            HeadMovementCheck.instance.StartHeadDetection();
            HeadMovementCheck.instance.onDetectionFinish.AddListener(DetectionResultHandler);
            headDetectionListenerAdded = true;
        }
        else if (!val)
        {
            HeadMovementCheck.instance.StopDetection(msg);
            HeadMovementCheck.instance.onDetectionFinish.RemoveListener(DetectionResultHandler);
            headDetectionListenerAdded = false;
        }
    }

// Handles the result from head detection
    private void DetectionResultHandler(DetectionResult result)
    {
        SetHeadDetectionCompleteListener(false, "finished");

        // Handle the result from head detection
        if (result.nodding)
        {
            GetNextTextObject(0);
        }
        else if (result.shaking)
        {
            GetNextTextObject(1);
        }
    }
    public void ResetTextProgress() // Resets the text
    {
        if (texts.Count == 0) {return;}
        if (texts[currentTextElementNumber].taskToComplete != null) {SetTaskCompleteListener(false);}
        if (headDetectionListenerAdded) {SetHeadDetectionCompleteListener(false, "Game reset");}
        currentTextElementNumber = 0;
        textStart = false;
        taskActive = false;
    }

    private void OnTaskComplete() // Wrapper for when the task is complete
    {
        if (debuggingEnabled)
        {
            Debug.Log($"{currentTextElementNumber}\n{texts[currentTextElementNumber].taskToComplete}");
        }
        SetTaskCompleteListener(false);
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
    [Tooltip("If this is true, the first option will be a nod to continue, second option will be a shake")]
    public bool headMovementOption = false;
    public List<TextOptions> options;
}

[System.Serializable]
public class TextOptions
{
    public string text;
    [Tooltip("To move on to the next text, set to -1. Else set it to the next text index")]
    public int jumpTo = -1;
    public int pointsToAward = 0;
}