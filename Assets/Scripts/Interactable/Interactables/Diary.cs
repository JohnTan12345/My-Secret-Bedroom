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
    [SerializeField]
    private GameObject closedDiary;
    [SerializeField]
    private GameObject openDiary;

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
        closedDiary.SetActive(true);
        openDiary.SetActive(false);
    }

    // Changes the text to what was selected
    public void ShowText(int choiceText)
    {
        textList[choiceText].SetActive(true);
        choiceGroup.SetActive(false);
    }

    public void OpenDiary()
    {
        closedDiary.SetActive(false);
        openDiary.SetActive(true);
    }
}
