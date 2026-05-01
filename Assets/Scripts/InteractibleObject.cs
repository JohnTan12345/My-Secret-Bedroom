/*
    Created by: John
    Description: Handles the informative in-game text parts of the game as well as handling the tasks if any
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleObject : MonoBehaviour
{
    public string interactibleName;
    public List<GameText> textFlow;
    private int currentTextElementNumber = 0;
    
    private bool textStart = false;
    // Events
    public UnityEvent onTextsStart;
    public UnityEvent onTextChange;
    public UnityEvent onTextsEnd;

    // Initialization
    void Awake()
    {
        
    }

    public GameText GetTextObject()
    {
        if (!textStart) {onTextsStart.Invoke(); currentTextElementNumber = 0; textStart = true;}
        return textFlow[currentTextElementNumber];
    }

    public GameText NextTextObject(int option = 0)
    {
        GameText currentText = textFlow[currentTextElementNumber];
        int nextTextElementNumber = currentTextElementNumber;
        if (currentText.options.Count == 0)
        {
            if (currentText.jumpTo > -1)
            {
                nextTextElementNumber = currentText.jumpTo;
            }
            else
            {
                nextTextElementNumber++;

                if (nextTextElementNumber == textFlow.Count)
                {
                    onTextsEnd.Invoke();
                    textStart = false;
                }
            }
        }
        else if (0 > option && option > currentText.options.Count)
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
        }

        return textFlow[currentTextElementNumber];
    }


}

[System.Serializable]
public class GameText
{
    public string text;
    public int jumpTo = -1;
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
