using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Mirror : MonoBehaviour
{
    public InteractableText interactableText;
    public InteractableTask interactableTask;
    public GameObject cloth;
    public GameObject textUI;
    private Vector3 clothOriginalPos;
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
        clothOriginalPos = cloth.transform.position;
    }
    private void GameReset()
    {
        cloth.GetComponent<XRGrabInteractable>().enabled = false;
        cloth.transform.position = clothOriginalPos;
        cloth.SetActive(true);
        textUI.SetActive(false);
    }
    private void TextFinished()
    {
        textUI.SetActive(false);
    }
    public void OnClothGrab()
    {
        interactableTask.AddProgress(1);
        cloth.SetActive(false);
    }
    public void OnPlayerEnterArea()
    {
        textUI.SetActive(true);
        cloth.GetComponent<XRGrabInteractable>().enabled = true;
    }
}
