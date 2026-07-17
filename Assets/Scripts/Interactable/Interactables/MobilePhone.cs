/*
    Created by: Lucas
    Modified by: John
    Description: Mobile phone interactable functions
*/

using UnityEngine;

public class MobilePhone : MonoBehaviour
{
    public InteractableText interactableText;
    public GameObject textUI;
    
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
    }

    private void GameReset() // Reset the object when the game reset is triggered
    {
        textUI.SetActive(false);
    }
    private void TextFinished() // Make the TextUI disappear
    {
        textUI.SetActive(false);
    }
    // Update is called once per frame
    public void OnPlayerEnterArea() // Make the TextUI appear when the player enters
    {
        textUI.SetActive(true);
    }
}
