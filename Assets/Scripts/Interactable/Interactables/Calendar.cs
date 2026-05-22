using UnityEngine;
using System.Collections;

public class Calendar : MonoBehaviour
{

    
    public Animator pageAnimator;

    public GameObject flipButton;

    private int currentPage = 0;

    private Quaternion originalRotation;

    public InteractableText interactableText;
    public InteractableTask interactableTask;

    public Transform pagePivot;

    public GameObject page;

    public GameObject textUI;

    public Texture2D[] calendarTextures;
    

    private Renderer pageRenderer;

    private bool taskFinished = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
        interactableText.onTextChange.AddListener(CheckCurrentText);
        pageRenderer = page.GetComponent<Renderer>();
        pageRenderer.material.mainTexture = calendarTextures[currentPage];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckCurrentText()
{
    GameText currentText = interactableText.GetGameText();

    if (currentText.taskToComplete == interactableTask)
    {
        flipButton.SetActive(true);
    }
    else
    {
        flipButton.SetActive(false);
    }
}

    private void GameReset()
    {
        textUI.SetActive(false);
        pagePivot.rotation = originalRotation;
        currentPage = 0;
        taskFinished = false;
        pageRenderer.material.mainTexture = calendarTextures[currentPage];
    }

    public void OnPlayerEnterArea()
    {
        textUI.SetActive(true);
    }

    public void TextFinished()
    {
        textUI.SetActive(false);
        taskFinished = true;
        Debug.Log("Text and task Finished");
    }

    private IEnumerator FlipPageRoutine()
{
    pageAnimator.Play("pageflipanimation", 0, 0f);

    yield return new WaitForSeconds(0.15f);

    currentPage++;

    if (currentPage >= calendarTextures.Length)
    {
        currentPage = calendarTextures.Length - 1;
        yield break;
    }

    pageRenderer.material.mainTexture = calendarTextures[currentPage];

    interactableTask.AddProgress(1);
}

    public void OnPageFlip()
{
    StartCoroutine(FlipPageRoutine());
}

    


}
