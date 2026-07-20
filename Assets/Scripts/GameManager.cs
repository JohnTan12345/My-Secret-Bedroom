/*
    Created by: John
    Description: Manages when the game starts, ends, resets as well as the settings for the player
*/

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Material highlightMaterial;

    // Game Settings
    [Header("Player Settings")]
    [SerializeField]
    public bool Movement = false;
    [SerializeField]
    public bool Position = false;

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
    private Vector3 sittingPosOffset;
    [SerializeField]
    private bool debuggingEnabled = false;
    
    void Awake()
    {
        // Set the instance to this for easier referencing
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        // Apply player settings after scene loads
        SceneManager.sceneLoaded += (Scene _, LoadSceneMode _) => ApplySettings();

        if (debuggingEnabled)
        {
            Debug.Log("AE");
        }
    }

    // Starts the game
    public async Task StartGame()
    {
        if (debuggingEnabled)
        {
            Debug.Log("Starting Game");
        }

        await SceneManager.LoadSceneAsync("Game");

        if (debuggingEnabled)
        {
            Debug.Log("Game Started");
        }
    }

    // Ends the game
    public async Task EndGame()
    {
        await SceneManager.LoadSceneAsync("MainMenu");
    }

    // Restarts the game
    public void ResetGame()
    {
        onGameReset.Invoke();

        AddPoints(-points); // Resets the points to 0
    }

    // Applies the settings player sets in the main menu
    public void ApplySettings(bool changeMovementSetting = true)
    {
        if (changeMovementSetting) // Movement setting application
        {
            XROriginMapping.instance.TeleportLocomotion.SetActive(!Movement);
            XROriginMapping.instance.MoveLocomotion.SetActive(Movement);
        }
        
        if (Position) // Position setting application
        {
            XROriginMapping.instance.CameraOffset.transform.position += sittingPosOffset;
        }
        else
        {
            XROriginMapping.instance.CameraOffset.transform.position -= sittingPosOffset;
        }
        
    }
}
