/*
    Created by: Rayner
    Description: Weighing scale interactable functions
*/

using UnityEngine;

public class Scale : MonoBehaviour
{

    public InteractableText interactableText;
    public GameObject questionUI;
    private bool interactionDone = false;
    
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(hideTextUI);

        questionUI.SetActive(false);
        
    }

    private void hideTextUI() // Make the TextBox appear and show the text
    {
        questionUI.SetActive(false);
        interactionDone = true;
    }

    public void OnPlayerEnterArea() // Make the questionUI appear when the player enters
    {
        if (interactionDone) {return;}
        questionUI.SetActive(true);
    }

    public void GameReset() // Disables the UI and reset variables to original
    {
        questionUI.SetActive(false);
        interactionDone = false;
    }

}
