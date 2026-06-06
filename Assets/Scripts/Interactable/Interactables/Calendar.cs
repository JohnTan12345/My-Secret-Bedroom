using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class Calendar : MonoBehaviour
{

    
    public XRSimpleInteractable calendarInteractable;
    public Animator pageAnimator;

    private int currentPage = 0;

    private Quaternion originalRotation;

    public InteractableText interactableText;
    public InteractableTask interactableTask;

    public Transform pagePivot;

    public GameObject page;

    public GameObject textUI;

    public Texture2D[] calendarTextures;
    
    private bool isFlipping = false;
    private Renderer pageRenderer;

    private bool taskFinished = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.onGameReset.AddListener(GameReset);
        interactableText.onTextsEnd.AddListener(TextFinished);
        pageRenderer = page.GetComponent<Renderer>();
        pageRenderer.material.mainTexture = calendarTextures[currentPage];
    }

    // Update is called once per frame
    void Update()
    {
        
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
    if (isFlipping)
        yield break;

    isFlipping = true;

    pageAnimator.Play("pageflipanimation", 0, 0f);

    yield return new WaitForSeconds(0.15f);

    currentPage++;

    if (currentPage >= calendarTextures.Length)
    {
        currentPage = calendarTextures.Length - 1;
        isFlipping = false;
        yield break;
    }

    pageRenderer.material.mainTexture = calendarTextures[currentPage];

    interactableTask.AddProgress(1);

    // Disable interaction after reaching the last page
    if (currentPage == calendarTextures.Length - 1)
    {
        calendarInteractable.enabled = false;
        Debug.Log("Calendar interaction disabled.");
    }

    yield return new WaitForSeconds(0.3f);

    isFlipping = false;
}

    public void OnPageFlip()
    {
        StartCoroutine(FlipPageRoutine());
    }

    


}
