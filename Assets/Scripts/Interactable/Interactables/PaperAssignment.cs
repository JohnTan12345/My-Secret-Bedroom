/*
    Created by: Xander
    Description: Paper assignment interactable functions
*/

using UnityEngine;


public class PaperAssignment : MonoBehaviour
{
    public InteractableText interactableText;

    public GameObject textUI;

    void Start() // Add listeners to relevant events
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
    }

    // Disable the UI on game reset
    private void GameReset()
    {
        textUI.SetActive(false);
        
    }

    // Enable the UI when player enters area
    public void OnPlayerEnterArea()
    {
        textUI.SetActive(true);
        
    }

    // Disable the UI when the text is finished
    public void TextFinished()
    {
        textUI.SetActive(false);
    }
}
