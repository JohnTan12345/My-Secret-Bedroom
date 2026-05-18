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
    public UnityEvent onPlayerAway;

    [Header("Hidden Parameters")]
    [SerializeField]
    private bool playerLooking;
    [SerializeField]
    private bool invokedLookedEvent;
    [SerializeField]
    private bool invokedAwayEvent;
    private Coroutine coroutine;

    public void OnLook()
    {
        playerLooking = true;
        if (coroutine == null)
        {
            coroutine = StartCoroutine(PlayerLookingCheck());
        }
    }

    private IEnumerator PlayerLookingCheck()
    {
        while (true)
        {
            if (playerLooking)
            {
                if (!invokedLookedEvent)
                {
                    onPlayerLook.Invoke();
                    invokedLookedEvent = true;
                    invokedAwayEvent = false;
                }
                yield return new WaitForEndOfFrame();
                playerLooking = false;
            }
            else
            {
                if (!invokedAwayEvent)
                {
                    onPlayerAway.Invoke();
                    invokedAwayEvent = true;
                    invokedLookedEvent = false;
                    coroutine = null;
                    yield break;
                }
                
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
