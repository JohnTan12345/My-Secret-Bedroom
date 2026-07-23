/*
    Template created by: John

    Script Created by: Rayner
    Description: Clothes pile interactivity and the basket visual
*/

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Clothes: BaseGameInteractables
{
    [Header("Interactable Variables")]
    [SerializeField]
    private GameObject clothingPile; // reference to the pile model
    [SerializeField]
    private Transform newTextUIPositon;
    [SerializeField]
    private GameObject clothesBasket;
    [SerializeField]
    private GameObject filledClothesBasket;

    private Vector3 pileOriginalPos;
    private Quaternion pileOriginalRot;
    private Vector3 textUIOriginalPos;
    private Quaternion textUIOriginalRot;
    
    private bool taskFinished = false;

    // DO NOT REMOVE THE base.function() OF ANY PROTECTED OVERRIDE LINE WITH IT!
    // THEY ARE THERE TO INITIALIZE THE SCRIPT
    protected override void Awake() // Initialization
    {
        base.Awake();

        textUIOriginalPos = textUI.transform.position;
        textUIOriginalRot = textUI.transform.rotation;
        pileOriginalPos = clothingPile.transform.position;
        pileOriginalRot = clothingPile.transform.rotation;
        textUI.SetActive(false);
    }

    protected override void OnPlayerEnterArea()
    {
        base.OnPlayerEnterArea();

        if (taskFinished) return;
        textUI.SetActive(true);
    }

    protected override void ResetInteractable() // Game Reset
    {
        // Put the pile back to original position
        clothingPile.SetActive(true);
        clothingPile.transform.position = pileOriginalPos;
        clothingPile.transform.rotation = pileOriginalRot;

        // Freezes the clothes pile
        clothingPile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        // Put the textUI back to original position
        textUI.transform.position = textUIOriginalPos;
        textUI.transform.rotation = textUIOriginalRot;

        // Make the pile not interactable
        clothingPile.GetComponent<XRGrabInteractable>().enabled = false;
        clothingPile.SetActive(true);

        // Disable text UI
        textUI.SetActive(false);

        taskFinished = false;

        // Resets the clothes basket back to empty
        filledClothesBasket.SetActive(false);
        clothesBasket.SetActive(true);
    }

    public void onStartTask()
    {
        clothingPile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        clothingPile.GetComponent<XRGrabInteractable>().enabled = true;
    }

    // Called when pile is dropped into laundry bag
    public void OnClothingDropped()
    {
        if (taskFinished) {return;}
        if (interactableTask != null)
        {
            interactableTask.AddProgress(1);
        }

        // fill up basket
        clothingPile.SetActive(false);
        clothesBasket.SetActive(false);
        filledClothesBasket.SetActive(true);

        clothingPile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        // Disable grabbing so player can’t pick it back up
        clothingPile.GetComponent<XRGrabInteractable>().enabled = false;

        taskFinished = true;
    }

    public void MoveUI()
    {
        // Move the UI to above the basket
        textUI.transform.position = newTextUIPositon.position;
        textUI.transform.rotation = newTextUIPositon.rotation;
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