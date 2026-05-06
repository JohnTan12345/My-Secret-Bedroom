using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    [SerializeField]
    [Tooltip("You can set an ordered list of interactible texts. Any interactible texts that is not added here will be automatically added with no order")]
    private List<InteractableText> interactibleTexts = new List<InteractableText>();

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
    public void AddPoints(int amount) { points += amount; onPointsChange.Invoke(); }
    public int GetPoints() => points;

    void Awake()
    {
        if (instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        
    }

    public void EndGame()
    {
        
    }

    public void ResetGame()
    {
        onGameReset.Invoke();

        points = 0;
        onPointsChange.Invoke();
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
