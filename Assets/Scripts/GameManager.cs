/*
    Created by: John
    Description: Manages when the game starts, ends, resets as well as the settings for the player
*/

using System.Collections;
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
    [SerializeField]
    private bool mainMenuFirstLoad = true;
    [SerializeField]
    private bool cameraPositionFirstApplied = false;
    
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
        mainMenuFirstLoad = false;
        await SceneManager.LoadSceneAsync("Game");
        OnSceneLoad();

        if (debuggingEnabled)
        {
            Debug.Log("Game Started");
        }
    }

    // Ends the game
    public async Task EndGame()
    {
        Debug.Log("Ending Game");
        mainMenuFirstLoad = true;
        await SceneManager.LoadSceneAsync("MainMenu");

        XROriginMapping.instance.MoveLocomotion.SetActive(false);
        XROriginMapping.instance.TeleportLocomotion.SetActive(false);
    }

    // Restarts the game
    public void ResetGame()
    {
        onGameReset.Invoke();

        AddPoints(-points); // Resets the points to 0
    }

    private void OnSceneLoad()
    {
        cameraPositionFirstApplied = false;
        StartCoroutine(ApplySettings());
    }
    
    // Applies the settings player sets in the main menu
    public IEnumerator ApplySettings(bool changeMovementSetting = true)
    {
        yield return new WaitForEndOfFrame();

        if (changeMovementSetting) // Movement setting application
        {
            XROriginMapping.instance.TeleportLocomotion.SetActive(!Movement);
            XROriginMapping.instance.MoveLocomotion.SetActive(Movement);
        }
        
        if (Position) // Position setting application
        {
            cameraPositionFirstApplied = true;
            XROriginMapping.instance.CameraOffset.transform.localPosition += sittingPosOffset;
        }
        else if (cameraPositionFirstApplied)
        {
            XROriginMapping.instance.CameraOffset.transform.localPosition -= sittingPosOffset;
        }
        
    }
}
