/*
    Created by: John
    Description: Base class for all game interactables
*/
using System.Collections;
using UnityEngine;

public abstract class BaseGameInteractables : MonoBehaviour
{
    protected EnterArea enterArea;
    protected GameObject textUI;
    protected InteractableText interactableText;
    protected virtual void Awake()
    {
        StartCoroutine(GameManagerEventListenerAdding());
    }

    private IEnumerator GameManagerEventListenerAdding()
    {
        yield return new WaitUntil(() => GameManager.instance != null);

        GameManager.instance.onGameReset.AddListener(ResetInteractable);
        interactableText.onTextsEnd.AddListener(TextEnded);

        if (enterArea != null)
        {
            enterArea.ObjectEnterArea.AddListener(OnPlayerEnterArea);
            enterArea.ObjectExitArea.AddListener(OnPlayerExitArea);
        }
    }

    protected virtual void OnDestroy()
    {
        GameManager.instance.onGameReset.RemoveListener(ResetInteractable);
        interactableText.onTextsEnd.RemoveListener(TextEnded);

        if (enterArea != null)
        {
            enterArea.ObjectEnterArea.RemoveListener(OnPlayerEnterArea);
            enterArea.ObjectExitArea.AddListener(OnPlayerExitArea);
        }
    }

    protected abstract void ResetInteractable();
    protected virtual void TextEnded()
    {
        textUI.SetActive(false);
    }

    protected virtual void OnPlayerEnterArea()
    {
        textUI.SetActive(true);

        if (interactableText.GetGameText().taskToComplete != null)
        {
            interactableText.GetGameText().taskToComplete.HighlightObject(true);
        }
    }

    protected virtual void OnPlayerExitArea()
    {
        textUI.SetActive(false);

        if (interactableText.GetGameText().taskToComplete != null)
        {
            interactableText.GetGameText().taskToComplete.HighlightObject(false);
        }
    }
}
