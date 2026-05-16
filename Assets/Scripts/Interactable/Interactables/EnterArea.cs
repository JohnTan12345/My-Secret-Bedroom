/*
    Created by: John
    Description: A wrapper for OnTriggerEnter that fires an event when the player enters the area
*/

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EnterArea : MonoBehaviour
{
    public UnityEvent PlayerEnterArea;
    public UnityEvent PlayerExitArea;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEnterArea.Invoke();
        };
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerExitArea.Invoke();
        };
    }
}
