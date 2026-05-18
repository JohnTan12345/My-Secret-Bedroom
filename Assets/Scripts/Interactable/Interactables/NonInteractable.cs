using UnityEngine;

public class NonInteractable : MonoBehaviour
{
    [SerializeField]
    private bool lookingAtObject = false;
    [SerializeField]
    private bool lookingAtUI = false;

    public GameObject TextUI;

    public void LookingAtObject(bool val)
    {
        if (val == lookingAtObject)
        {
            return;
        }

        lookingAtObject = val;
        UpdateUI();
    }

    public void LookingAtUI(bool val)
    {
        if (val == lookingAtUI)
        {
            return;
        }

        lookingAtUI = val;
        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log(lookingAtObject || lookingAtUI);
        if ((TextUI.activeSelf && (lookingAtObject || lookingAtUI)) || (!TextUI.activeSelf && lookingAtObject))
        {
            Debug.Log("smth");
            TextUI.SetActive(true);
        }
        else
        {
            TextUI.SetActive (false);
        }
    }
}
