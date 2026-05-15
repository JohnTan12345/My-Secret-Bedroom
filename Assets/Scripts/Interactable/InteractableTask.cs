/*
    Created by: John
    Description: Handles the task for the game
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTask : MonoBehaviour
{
    public int MaxProgress = 1;
    public GameObject ObjectHighlight;

    [Header("Hidden Parameters")]
    [SerializeField]
    private int currentProgress = 0;

    // Events
    [Header("Events")]
    [Space(5)]
    public UnityEvent onTaskStart; // Fires when the task starts
    public UnityEvent onTaskComplete; // Fires when the task is complete

    void Awake()
    {   
        // Check if the progress is set up correctly
        if (MaxProgress <= 0)
        {
            throw new System.Exception("Max Progress cannot be 0 or less");
        }

        StartCoroutine(WaitForGameManagerInstance());

        onTaskStart.AddListener(() => {Debug.Log("Task started");});
    }

    private IEnumerator WaitForGameManagerInstance()
    {
        // Wait for the game manager to load before subscribing to onGameReset
        yield return new WaitUntil(() => GameManager.instance != null);
        GameManager.instance.onGameReset.AddListener(ResetProgress);
    }

    public void AddProgress(int amount)
    {
        currentProgress += amount; // Adds to the current progress the given amount

        if (currentProgress >= MaxProgress) // Check if current progress is more than the max progress
        {
            onTaskComplete.Invoke(); // Fires the task complete event
            ResetProgress(); // Resets the progress (may be removed)
        }
    }
    public void ResetProgress()
    {
        currentProgress = 0; // Resets progress to 0
    }
}
