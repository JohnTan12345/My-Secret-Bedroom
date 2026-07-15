/*
    Created by: Xander
    Description: Calendar animations and functions
*/

using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class Calendar : MonoBehaviour
{
    public XRSimpleInteractable calendarInteractable;
    public Animator pageAnimator;

    private int currentPage = 0;

    private Quaternion originalRotation;

    public InteractableText interactableText;
    public InteractableTask interactableTask;

    public Transform pagePivot;

    public GameObject page;

    public GameObject textUI;

    public Texture2D[] calendarTextures;
    
    private bool isFlipping = false;
    private Renderer pageRenderer;

    private bool taskFinished = false;

    // Assign variables and event listeners
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
        pageRenderer = page.GetComponent<Renderer>();
        pageRenderer.material.mainTexture = calendarTextures[currentPage];
    }

    // Reset the interactable to original position
    private void GameReset()
    {
        textUI.SetActive(false);
        pagePivot.rotation = originalRotation;
        currentPage = 0;
        taskFinished = false;
        pageRenderer.material.mainTexture = calendarTextures[currentPage];
        calendarInteractable.enabled = false;
    }

    // Start the task and highlight the calendar when player enters
    public void OnPlayerEnterArea()
    {
        textUI.SetActive(true);
        if (!taskFinished)
        {
            calendarInteractable.enabled = true;
            interactableTask.HighlightObject(true);
        }
    }

    // Unhighlight the calendar and disable the text UI when player leaves
    public void OnPlayerExitArea()
    {
        textUI.SetActive(false);
        calendarInteractable.enabled = false;
        interactableTask.HighlightObject(false);
    }

    // Disable the text UI when the text is done
    public void TextFinished()
    {
        textUI.SetActive(false);
        Debug.Log("Text and task Finished");
    }

    // Make the calendar non interactable when task is done
    public void TaskFinished()
    {
        taskFinished = true;
        calendarInteractable.enabled = false;
    }

    // Page flip animation
   private IEnumerator FlipPageRoutine()
    {
        if (isFlipping) // Check if page is in the middle of animation
            yield break;
            
        isFlipping = true;

        pageAnimator.Play("pageflipanimation", 0, 0f); // Play animation

        yield return new WaitForSeconds(0.15f); // Wait for animation to finish

        currentPage++;
        
        // Check if its the last page
        if (currentPage >= calendarTextures.Length)
        {
            currentPage = calendarTextures.Length - 1;
            isFlipping = false;
            yield break;
        }

        // Change texture and add task progress after page flip
        pageRenderer.material.mainTexture = calendarTextures[currentPage];

        interactableTask.AddProgress(1);

        yield return new WaitForSeconds(0.3f);

        isFlipping = false;
    }

    // When the calendar is interacted
    public void OnPageFlip()
    {
        StartCoroutine(FlipPageRoutine());
    }

}
