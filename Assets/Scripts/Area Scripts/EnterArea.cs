/*
    Created by: John
    Description: Area players can enter to trigger something
*/

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EnterArea : MonoBehaviour
{
    [SerializeField]
    private string focusedTag = "Player";
    public UnityEvent ObjectEnterArea;
    public UnityEvent ObjectExitArea;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(focusedTag))
        {
            ObjectEnterArea.Invoke();
        };
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(focusedTag))
        {
            ObjectExitArea.Invoke();
        };
    }
}
