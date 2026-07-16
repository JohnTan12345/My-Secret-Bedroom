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

    // Resets variables to default
    void OnDisable()
    {
        onPlayerAway.Invoke();
        invokedAwayEvent = true;
        invokedLookedEvent = false;
        coroutine = null;
    }

    // When the player looks
    public void OnLook()
    {
        playerLooking = true;

        if (coroutine == null) // Check if theres an active looking check
        {
            coroutine = StartCoroutine(PlayerLookingCheck());
        }
    }

    private IEnumerator PlayerLookingCheck()
    {
        while (true)
        {
            // If the Look Area is disabled, stop the check
            if (!gameObject.activeSelf)
            {
                yield break;
            }

            if (playerLooking) // The moment the player starts looking
            {
                if (!invokedLookedEvent) // If the event is yet to be fired
                {
                    onPlayerLook.Invoke();
                    invokedLookedEvent = true;
                    invokedAwayEvent = false;
                }
                yield return new WaitForEndOfFrame(); // Wait for the frame to end to check if the player is looking
                playerLooking = false;
            }
            else // Keep checking for when the player looks away
            {
                if (!invokedAwayEvent) // If the event is yet to be fired
                {
                    onPlayerAway.Invoke();
                    invokedAwayEvent = true;
                    invokedLookedEvent = false;
                    coroutine = null;
                    yield break; // Stop the check after player looks away
                }
                
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
