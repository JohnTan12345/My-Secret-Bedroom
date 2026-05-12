using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    [SerializeField]
    [Tooltip("You can set an ordered list of interactable texts. Any interactable texts that is not added here will be automatically added with no order")]
    private List<InteractableText> interactableTexts = new List<InteractableText>();

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

    [Header("Hidden Parameters")]
    [SerializeField]
    private int activeInteractableNum = 0;

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

    public void StartGame()
    {
        SetActiveInteractableNum(0);
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

    void SetActiveInteractableNum(int interactableNum)
    {
        interactableTexts[activeInteractableNum].onTextsEnd.RemoveListener(OnTextsFinish);
        // Previous highlighted object becomes unhighlighted

        activeInteractableNum = interactableNum;
        interactableTexts[activeInteractableNum].onTextsEnd.AddListener(OnTextsFinish);
        // Next object becomes highlighted
    }

    private void OnTextsFinish()
    {
        SetActiveInteractableNum(activeInteractableNum + 1);
    }

    public bool AddInteractableText(InteractableText interactableText)
    {
        if (interactableTexts.Contains(interactableText))
        {
            Debug.Log($"{interactableText} is already inside {instance}");
            return false;
        }

        interactableTexts.Add(interactableText);
        return true;
    }
}
