using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ClothesPile : MonoBehaviour
{
    public InteractableText interactableText;
    public InteractableTask interactableTask;
    public GameObject questionUI;
    public GameObject clothingPile; // reference to the pile model

    public string clothingTag = "ClothingPile"; 
    public Collider laundryBagTrigger;          

    private bool interactionDone = false;
    private Vector3 pileOriginalPos;
    private bool taskFinished = false;

    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);

        pileOriginalPos = clothingPile.transform.position;
        questionUI.SetActive(false);
    }

    public void OnPlayerEnterArea()
    {
        if (taskFinished) return;
        questionUI.SetActive(true);
        clothingPile.GetComponent<XRGrabInteractable>().enabled = true;
    }

    // Called when pile is dropped into laundry bag
    public void OnClothingDropped()
    {
        if (interactableTask != null)
        {
            interactableTask.AddProgress(1);
        }

        Debug.Log("Disabling: " + clothingPile.name);
        clothingPile.SetActive(false); // hide pile after use

        interactionDone = true;
        taskFinished = true;

        // 🔹 Advance the dialogue only after pile is dropped
        if (interactableText != null)
        {
            interactableText.SelectOption(-1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.name}, Tag: {other.tag}");
        if (other.CompareTag(clothingTag))
        {
            Debug.Log("Clothing pile dropped into laundry bag!");
            OnClothingDropped();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(clothingTag))
        {
            Debug.Log("Clothing pile removed from laundry bag.");
        }
    }

    public void GameReset()
    {
        clothingPile.GetComponent<XRGrabInteractable>().enabled = false;
        clothingPile.transform.position = pileOriginalPos;
        clothingPile.SetActive(true);

        questionUI.SetActive(false);

        interactionDone = false;
        taskFinished = false;
    }
}
