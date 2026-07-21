/*
    Template created by: John

    Script Created by: Xander
    Description: Calendar animations and functions
*/

using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Calendar: BaseGameInteractables
{
    [Header("Interactable Variables")]
    public XRSimpleInteractable calendarInteractable;
    public Animator pageAnimator;
    public Transform pagePivot;
    public GameObject page;
    private int currentPage = 0;

    private Quaternion originalRotation;
    
    public Texture2D[] calendarTextures;
    
    private bool isFlipping = false;
    private Renderer pageRenderer;

    private bool taskFinished = false;

    // DO NOT REMOVE THE base.function() OF ANY PROTECTED OVERRIDE LINE WITH IT!
    // THEY ARE THERE TO INITIALIZE THE SCRIPT
    protected override void Awake() // Initialization
    {
        base.Awake();

        pageRenderer = page.GetComponent<Renderer>();
        pageRenderer.material.mainTexture = calendarTextures[currentPage];
    }

    protected override void ResetInteractable() // Reset the interactable to original position
    {
        textUI.SetActive(false);
        pagePivot.rotation = originalRotation;
        currentPage = 0;
        taskFinished = false;
        pageRenderer.material.mainTexture = calendarTextures[currentPage];
        calendarInteractable.enabled = false;
    }

    protected override void OnPlayerEnterArea() // Start the task and highlight the calendar when player enters
    {
        base.OnPlayerEnterArea();

        if (!taskFinished)
        {
            calendarInteractable.enabled = true;
        }
    }
    protected override void OnPlayerExitArea()
    {
        base.OnPlayerExitArea();

        calendarInteractable.enabled = false;
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

// Optional functions

// Use this template for it:

/*
protected override void function()
{
    base.function();

    // Your code here
}
*/

// Replace function() with any optional function below

// TextEnded()
// OnPlayerEnterArea()
// OnPlayerExitArea()