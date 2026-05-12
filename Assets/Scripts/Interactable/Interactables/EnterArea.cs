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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEnterArea.Invoke();
        };
    }
}
