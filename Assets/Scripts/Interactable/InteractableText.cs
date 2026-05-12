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

    [Header("Events")]
    [Space(5)]
    // Events
    public UnityEvent onTextsStart;
    public UnityEvent onTextChange;
    public UnityEvent onTextsEnd;

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
        bool success = GameManager.instance.AddInteractableText(this);

        if (success)
        {
            Debug.Log("Successfully added interactible text to game manager");
        }
    }
    
    public GameText GetGameText()
    {
        if (!textStart) {onTextsStart.Invoke(); currentTextElementNumber = 0; textStart = true;}
        if (!taskActive && texts[currentTextElementNumber].taskToComplete != null) {taskActive = true; texts[currentTextElementNumber].taskToComplete.onTaskComplete.AddListener(OnTaskComplete);}
        return texts[currentTextElementNumber];
    }

// Text related Functions

    public void SelectOption(int option = -1)
    {
        GetNextTextObject(option);
    }

    private GameText GetNextTextObject(int option)
    {

        if (taskActive == true)
        {
            Debug.LogWarning("There is a task currently active");
            return texts[currentTextElementNumber];
        }

        GameText currentText = texts[currentTextElementNumber];
        int nextTextElementNumber = currentTextElementNumber + 1;

        if (currentText.options.Count == 0)
        {
            if (currentText.jumpTo > -1)
            {
                nextTextElementNumber = currentText.jumpTo;
            }
            else
            {
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
                throw new System.Exception($"Invalid text number to jump to\nText Element Number: {currentTextElementNumber}\nOption given: {option}\nOption to jump to: {currentText.options[option].jumpTo}");
            }
            else
            {
                nextTextElementNumber = chosenOption.jumpTo;
            }

            GameManager.instance.AddPoints(chosenOption.pointsToAward);
        }
        else
        {
            throw new System.Exception($"Invalid option given\nText Element Number: {currentTextElementNumber}\nTotal option amount: {currentText.options.Count}\nOption given: {option}");
        }

        if (currentTextElementNumber != nextTextElementNumber)
        {
            currentTextElementNumber = nextTextElementNumber;
            onTextChange.Invoke();

            if (texts[currentTextElementNumber].taskToComplete != null)
            {
                taskActive = true;
                texts[currentTextElementNumber].taskToComplete.onTaskComplete.AddListener(OnTaskComplete);
            }
        }

        return texts[currentTextElementNumber];
    }

    private void ResetTextProgress()
    {
        currentTextElementNumber = 0;
        textStart = false;
        taskActive = false;
    }

    private void OnTaskComplete()
    {
        texts[currentTextElementNumber].taskToComplete.onTaskComplete.RemoveListener(OnTaskComplete);
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