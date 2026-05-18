using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ClothesPile : MonoBehaviour
{
    public InteractableText interactableText;
    public InteractableTask interactableTask;
    public GameObject questionUI;
    public GameObject nextUIPanel;
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

        Debug.Log("Placing pile into laundry bag: " + clothingPile.name);

        // Move pile into the bag’s position
        clothingPile.transform.position = laundryBagTrigger.transform.position;
        clothingPile.transform.rotation = Quaternion.identity;
        clothingPile.transform.SetParent(laundryBagTrigger.transform);

        // Disable grabbing so player can’t pick it back up
        clothingPile.GetComponent<XRGrabInteractable>().enabled = false;

        interactionDone = true;
        taskFinished = true;

        // Advance dialogue
        if (interactableText != null)
        {
            interactableText.SelectOption(-1);
        }

        Debug.Log("Clothing pile successfully dropped into laundry bag and task progress updated.");

        // Switch UI panels
        questionUI.SetActive(false);
        if (nextUIPanel != null)
        {
            nextUIPanel.SetActive(true);

            // Start the text sequence on the new panel
            var textScript = nextUIPanel.GetComponent<InteractableText>();
            if (textScript != null)
            {
                textScript.GetGameText();
            }
            Debug.Log("Next UI panel activated and text sequence started.");
        }
        Debug.Log("Clothing pile successfully dropped into laundry bag and interaction completed.");
    }

    private void OnTriggerEnter(Collider other)
    {
    Debug.Log($"Trigger entered by: {other.name}, Tag: {other.tag}, Root: {other.transform.root.name}");

    if (other.CompareTag(clothingTag) || other.transform.root.CompareTag(clothingTag))
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
    clothingPile.transform.SetParent(null); // detach from bag
    clothingPile.transform.position = pileOriginalPos;
    clothingPile.transform.rotation = Quaternion.identity;

    //clothingPile.GetComponent<XRGrabInteractable>().enabled = false;
    clothingPile.SetActive(true);

    questionUI.SetActive(false);
    if (nextUIPanel != null) nextUIPanel.SetActive(false);

    interactionDone = false;
    taskFinished = false;
    }

}
