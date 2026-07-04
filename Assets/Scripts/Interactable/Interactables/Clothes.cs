using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Clothes : MonoBehaviour
{
    [SerializeField]
    private InteractableText interactableText;
    [SerializeField]
    private InteractableTask interactableTask;
    [SerializeField]
    private GameObject textUI;
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

    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        textUIOriginalPos = textUI.transform.position;
        textUIOriginalRot = textUI.transform.rotation;
        pileOriginalPos = clothingPile.transform.position;
        pileOriginalRot = clothingPile.transform.rotation;
        textUI.SetActive(false);
    }

    public void OnPlayerEnterArea()
    {
        if (taskFinished) return;
        textUI.SetActive(true);
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
        // Switch UI panels
        textUI.transform.position = newTextUIPositon.position;
        textUI.transform.rotation = newTextUIPositon.rotation;
    }

    public void GameReset()
    {
        clothingPile.SetActive(true);
        clothingPile.transform.position = pileOriginalPos;
        clothingPile.transform.rotation = pileOriginalRot;

        clothingPile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        textUI.transform.position = textUIOriginalPos;
        textUI.transform.rotation = textUIOriginalRot;

        clothingPile.GetComponent<XRGrabInteractable>().enabled = false;
        clothingPile.SetActive(true);

        textUI.SetActive(false);

        taskFinished = false;

        filledClothesBasket.SetActive(false);
        clothesBasket.SetActive(true);
    }

}
