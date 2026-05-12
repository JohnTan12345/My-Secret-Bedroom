/*
    Created by: John
    Description: Dairy Interactable
*/

using TMPro;
using UnityEngine;

public class Diary : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    
    [SerializeField]
    private GameObject choiceGroup;

    // Changes the text to what was selected
    public void InsertText(string choiceText)
    {
        text.text = choiceText;
        text.gameObject.SetActive(true);
        choiceGroup.SetActive(false);
    }
}
