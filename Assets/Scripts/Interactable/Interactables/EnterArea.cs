/*
    Created by: John
    Description: Area players can enter to trigger something
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
