/*
    Template created by: John

    Script Created by: John
    Description: Mirror functions
*/

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Mirror: BaseGameInteractables
{
    [Header("Interactable Variables")]
    public GameObject cloth;
    private Vector3 clothOriginalPos;
    private bool taskFinished = false;


    // DO NOT REMOVE THE base.function() OF ANY PROTECTED OVERRIDE LINE WITH IT!
    // THEY ARE THERE TO INITIALIZE THE SCRIPT
    protected override void Awake() // Initialization
    {
        base.Awake();

        clothOriginalPos = cloth.transform.position;
    }

    protected override void ResetInteractable() // Game Reset
    {
        cloth.transform.position = clothOriginalPos;
        cloth.SetActive(true);
        taskFinished = false;
    }

    protected override void TextEnded() // Make the TextUI disappear
    {
        base.TextEnded();

        taskFinished = true;
    }

    public void OnClothGrab() // Complete the task after the cloth is grabbed
    {
        interactableTask.AddProgress(1);
        cloth.SetActive(false);
    }

    protected override void OnPlayerEnterArea() // Make the cloth interactable when the player enters
    {
        base.OnPlayerEnterArea();

        if (taskFinished) {return;}
        cloth.GetComponent<XRGrabInteractable>().enabled = true;
    }

    protected override void OnPlayerExitArea() // Make the cloth non-interactable when the player exits
    {
        base.OnPlayerExitArea();

        cloth.GetComponent<XRGrabInteractable>().enabled = false;
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