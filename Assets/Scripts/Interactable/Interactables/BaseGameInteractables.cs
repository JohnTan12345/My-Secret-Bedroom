/*
    Created by: John
    Description: Base class for all game interactables
*/
using System.Collections;
using UnityEngine;

public abstract class BaseGameInteractables : MonoBehaviour
{
    // Mandatory variables
    [SerializeField]
    protected GameObject textUI;
    [SerializeField]
    protected InteractableText interactableText;

    // Optional variables
    [Header("Optional")]
    [SerializeField]
    [Tooltip("The interactable area")]
    protected EnterArea enterArea;

    // Internal variables
    private protected InteractableTask interactableTask;

    protected virtual void Awake() // Initialization
    {
        StartCoroutine(GameManagerEventListenerAdding());

        // Event listeners adding
        interactableTask = interactableText.GetGameText().taskToComplete;

        interactableText.onTextsEnd.AddListener(TextEnded);
        interactableText.onTextChange.AddListener(SetInteractableTask);

        if (enterArea != null)
        {
            enterArea.ObjectEnterArea.AddListener(OnPlayerEnterArea);
            enterArea.ObjectExitArea.AddListener(OnPlayerExitArea);
        }
    }

    private IEnumerator GameManagerEventListenerAdding() // Initialization for Game Manager events
    {
        yield return new WaitUntil(() => GameManager.instance != null);

        GameManager.instance.onGameReset.AddListener(ResetInteractable);
    }

    protected virtual void OnDestroy() // Cleanup
    {
        GameManager.instance.onGameReset.RemoveListener(ResetInteractable);
        interactableText.onTextsEnd.RemoveListener(TextEnded);
        interactableText.onTextChange.RemoveListener(SetInteractableTask);

        if (enterArea != null)
        {
            enterArea.ObjectEnterArea.RemoveListener(OnPlayerEnterArea);
            enterArea.ObjectExitArea.AddListener(OnPlayerExitArea);
        }
    }

    // Resets the interactable to original position
    protected abstract void ResetInteractable();

    // Disables the text UI after the texts ends
    protected virtual void TextEnded()
    {
        textUI.SetActive(false);
    }

    // Enables the UI and highlights task objects when the player enters the interactable area
    protected virtual void OnPlayerEnterArea()
    {
        textUI.SetActive(true);

        if (interactableTask != null)
        {
            interactableTask.HighlightObject(true);
        }
    }

    // Disables the UI and unhighlights task objects when the player enters the interactable area
    protected virtual void OnPlayerExitArea()
    {
        textUI.SetActive(false);

        if (interactableTask != null)
        {
            interactableTask.HighlightObject(false);
        }
    }

    // Sets the interactable task
    private void SetInteractableTask()
    {
        interactableTask = interactableText.GetGameText().taskToComplete;
    }
}
