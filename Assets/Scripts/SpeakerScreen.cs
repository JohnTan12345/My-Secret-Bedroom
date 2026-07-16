/*
    Created by: Lucas
    Modified by: John
    Description: Bluetooth speaker screen switching
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeakerScreen : MonoBehaviour
{
    public List<Sprite> spriteList;     // Drag your sprites here
    public Image screenImage;           // Drag the UI Image here
    public float changeInterval = 2f;   // Seconds between changes

    private int currentIndex = 0;

    void OnEnable()
    {
        if (spriteList.Count > 0)
        {
            screenImage.sprite = spriteList[currentIndex];
            // Switch the sprites repeatedly 
            InvokeRepeating("NextSprite", changeInterval, changeInterval);
        }
    }

    void OnDisable() // Just in case of memory leaks
    {
        CancelInvoke();
    }

    void NextSprite()
    {
        currentIndex++;
        if (currentIndex >= spriteList.Count)
            currentIndex = 0; // loop back

        // Change the screen based on the current index
        screenImage.sprite = spriteList[currentIndex];
    }
}
