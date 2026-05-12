/*
    Created by: John
    Description: Dairy Interactable
*/

using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> textList = new();
    
    [SerializeField]
    private GameObject choiceGroup;

    void Start()
    {
        GameManager.instance.onGameReset.AddListener(ResetObject);
    }

    // Resets the object
    private void ResetObject()
    {
        foreach (GameObject text in textList)
        {
            text.SetActive(false);
        }
        choiceGroup.SetActive(true);
    }

    // Changes the text to what was selected
    public void ShowText(int choiceText)
    {
        textList[choiceText].SetActive(true);
        choiceGroup.SetActive(false);
    }
}
