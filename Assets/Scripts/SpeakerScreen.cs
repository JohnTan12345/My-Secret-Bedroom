using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeakerScreen : MonoBehaviour
{
    public List<Sprite> spriteList;     // Drag your sprites here
    public Image screenImage;           // Drag the UI Image here
    public float changeInterval = 2f;   // Seconds between changes

    private int currentIndex = 0;

    void Start()
    {
        if (spriteList.Count > 0)
        {
            screenImage.sprite = spriteList[currentIndex];
            InvokeRepeating("NextSprite", changeInterval, changeInterval);
        }
    }

    void NextSprite()
    {
        currentIndex++;
        if (currentIndex >= spriteList.Count)
            currentIndex = 0; // loop back

        screenImage.sprite = spriteList[currentIndex];
    }
}
