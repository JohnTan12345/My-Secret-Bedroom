    /*
    Created by: John
    Description: Mirror functions
*/

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Mirror : MonoBehaviour
{
    public InteractableText interactableText;
    public InteractableTask interactableTask;
    public GameObject cloth;
    public GameObject textUI;
    private Vector3 clothOriginalPos;
    private bool taskFinished = false;
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
        clothOriginalPos = cloth.transform.position;
    }
    private void GameReset() // Reset the object when the game reset is triggered
    {
        cloth.GetComponent<XRGrabInteractable>().enabled = false;
        cloth.transform.position = clothOriginalPos;
        cloth.SetActive(true);
        textUI.SetActive(false);
        taskFinished = false;
    }
    private void TextFinished() // Make the TextUI disappear
    {
        textUI.SetActive(false);
        taskFinished = true;
    }
    public void OnClothGrab() // Complete the task after the cloth is grabbed
    {
        interactableTask.AddProgress(1);
        cloth.SetActive(false);
    }
    public void OnPlayerEnterArea() // Make the TextUI appear when the player enters
    {
        if (taskFinished) {return;}
        textUI.SetActive(true);
        cloth.GetComponent<XRGrabInteractable>().enabled = true;
    }
}
