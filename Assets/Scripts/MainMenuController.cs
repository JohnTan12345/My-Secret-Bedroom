using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    private GameObject main;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject credits;

    [Header("Debugging")]
    [SerializeField]
    private bool resetMenus = true;

    void Awake()
    {
        if (resetMenus)
        {
            main.SetActive(true);
            settings.SetActive(false);
            credits.SetActive(false);
        }
    }

    public void StartGame()
    {
        GameManager.instance.StartGame();
    }

    // Menu interactions
    public void ChangeMenu(GameObject menu)
    {
        main.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(false);

        menu.SetActive(true);
    }

    public void ReturnToMain()
    {
        main.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
    }

    // Settings Changes
    public void ChangeMovementType()
    {
        // Change Movement Type
    }

    public void ChangeSeatedMode()
    {
        // Change seated mode
    }
}
