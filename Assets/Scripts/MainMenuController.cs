using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    private GameObject main;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject credits;

    [Header("Settings Sliders")]
    [SerializeField]
    private Slider movementSlider;
    [SerializeField]
    private Slider positionSlider;

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

        movementSlider.onValueChanged.AddListener(ChangeMovementType);
        positionSlider.onValueChanged.AddListener(ChangePositionType);
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
    public void ChangeMovementType(float val)
    {
        if (val == 0)
        {
            GameManager.instance.Movement = false;
        }
        else
        {
            GameManager.instance.Movement = true;
        }
        
    }

    public void ChangePositionType(float val)
    {
        if (val == 0)
        {
            GameManager.instance.Position = false;
        }
        else
        {
            GameManager.instance.Position = true;
        }
    }
}
