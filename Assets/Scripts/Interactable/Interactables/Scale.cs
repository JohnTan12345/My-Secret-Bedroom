    /*
    Created by: Rayner
    Description: WeighingScale functions
*/

using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{

    public InteractableText interactableText;
    public GameObject questionUI;
    private bool interactionDone = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void GameReset()
    {
        questionUI.SetActive(false);
        interactionDone = false;
    }

}
