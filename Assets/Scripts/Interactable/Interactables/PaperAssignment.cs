using UnityEngine;


public class PaperAssignment : MonoBehaviour
{
    public InteractableText interactableText;

    public GameObject textUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameReset()
    {
        textUI.SetActive(false);
        
    }

    public void OnPlayerEnterArea()
    {
        textUI.SetActive(true);
        
    }

    public void TextFinished()
    {
        textUI.SetActive(false);
    }
}
