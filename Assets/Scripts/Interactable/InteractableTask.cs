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
    public UnityEvent onTaskComplete;

    void Awake()
    {
        if (MaxProgress <= 0)
        {
            throw new System.Exception("Max Progress cannot be 0 or less");
        }

        StartCoroutine(WaitForGameManagerInstance());
    }

    private IEnumerator WaitForGameManagerInstance()
    {
        // Wait for the game manager to load before subscribing to onGameReset
        yield return new WaitUntil(() => GameManager.instance != null);
        GameManager.instance.onGameReset.AddListener(ResetProgress);
    }

    public void AddProgress(int amount)
    {
        currentProgress += amount;

        if (currentProgress >= MaxProgress)
        {
            onTaskComplete.Invoke();
            ResetProgress();    
        }
    }

    public void ResetProgress()
    {
        currentProgress = 0;
    }
}
