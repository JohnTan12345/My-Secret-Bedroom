/*
    Created by: John
    Description: Manages the overall game as well as resets
*/

using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Material highlightMaterial;

    // Game Settings
    public bool Teleport {get; private set;} = false;
    public bool Seated {get; private set;} = false;

    // Player Stats
    [Header("Player Stats")]
    [SerializeField]
    // Points
    private int points = 0;
    public void AddPoints(int amount) { points += amount; onPointsChange.Invoke(); } // Adds points
    public int GetPoints() => points; // Returns points

    // Events
    [Header("Events")]
    [Space(5)]
    public UnityEvent onGameReset;
    public UnityEvent onPointsChange;

    [Header("Hidden Parameters")]
    [SerializeField]
    private bool debuggingEnabled = false;
    
    void Awake()
    {
        AddPoints(-points); // Resets the points to 0

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
        if (debuggingEnabled)
        {
            Debug.Log("Game Started");
        }
    }

    public void EndGame()
    {
        
    }

    public void ResetGame()
    {
        onGameReset.Invoke();

        AddPoints(-points); // Resets the points to 0
    }
}
