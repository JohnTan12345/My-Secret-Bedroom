/*
    Created by: John
    Description: Manages the overall game as well as resets
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    [SerializeField]
    [Tooltip("You can set an ordered list of interactable texts. Any interactable texts that is not added here will be automatically added with no order")]
    private List<InteractableText> interactableTexts = new List<InteractableText>();
    public Material HighlightMat;

    // Events
    [Header("Events")]
    [Space(5)]
    public UnityEvent onGameReset;
    public UnityEvent onPointsChange;

    // Player Stats
    [Header("Player Stats")]
    [SerializeField]
    // Points
    private int points = 0;
    public void AddPoints(int amount) { points += amount; onPointsChange.Invoke(); } // Adds points
    public int GetPoints() => points; // Returns points

    [Header("Hidden Parameters")]
    [SerializeField]
    private int activeInteractableNum = 0;
    [SerializeField]
    private List<InteractableText> completedText = new();
    [SerializeField]
    private List<InteractableText> incompleteText = new();
    [SerializeField]
    private bool debuggingEnabled = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void StartGame() // To be implemented
    {
        incompleteText = new(interactableTexts);
        completedText = new() {};

        SetActiveInteractableNum(0);
        
        if (debuggingEnabled)
        {
            Debug.Log($"Game started");
        }
    }

    public void EndGame()
    {
        // Show thanks and stuff
    }

    public void ResetGame()
    {
        onGameReset.Invoke();

        points = 0;
        onPointsChange.Invoke();
    }

    private void OnTextsFinish()
    {
        InteractableText interactable = interactableTexts[activeInteractableNum];
        if (!completedText.Contains(interactable))
        {
            completedText.Add(interactable);
            incompleteText.Remove(interactable);
        }

        SetActiveInteractableNum(activeInteractableNum + 1);
    }

    private void SetActiveInteractableNum(int interactableNum)
    {
        interactableTexts[activeInteractableNum].onTextsEnd.RemoveListener(OnTextsFinish);
        // Previous highlighted object becomes unhighlighted

        activeInteractableNum = interactableNum;
        interactableTexts[activeInteractableNum].onTextsEnd.AddListener(OnTextsFinish);
        // Next object becomes highlighted
    }

    public void AddInteractableText(InteractableText interactableText) // Adds the interactable to the list for the game to easily order the tasks
    {
        if (interactableTexts.Contains(interactableText)) // Check if the interactableText is inside the list
        {
            if (debuggingEnabled)
            {
                Debug.Log($"{interactableText} is already inside {instance}");
            }
            
            return;
        }

        interactableTexts.Add(interactableText); // Add if the interactableText is not inside the list

        if (debuggingEnabled)
            {
                Debug.Log($"{interactableText} successfully added to {instance}");
            }
    }
}
