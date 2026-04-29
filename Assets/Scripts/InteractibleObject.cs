using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleObject : MonoBehaviour
{
    public string interactibleName;
    public List<GameText> textFlow;
    public int currentTextElementNumber = 0;
    
    // Events
    public UnityEvent onTextsStart;
    public UnityEvent onTextChange;
    public UnityEvent onTextsFinished;
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
