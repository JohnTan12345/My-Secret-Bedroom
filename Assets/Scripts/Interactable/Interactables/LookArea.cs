/*
    Created by: John
    Description: Area players can look to trigger something
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LookArea : MonoBehaviour
{
    public UnityEvent onPlayerLook;

    [Header("Hidden Parameters")]
    [SerializeField]
    private bool playerLooking;
    [SerializeField]
    private bool invokedEvent;
    void Start()
    {
        StartCoroutine(PlayerLookingCheck());
    }
    public void OnLook()
    {
        playerLooking = true;
    }

    private IEnumerator PlayerLookingCheck()
    {
        while (true)
        {
            if (playerLooking)
            {
                if (!invokedEvent)
                {
                    Debug.Log("Event Invoked");
                    invokedEvent = true;
                }
                yield return new WaitForEndOfFrame();
                playerLooking = false;
            }
            else
            {
                invokedEvent = false;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
