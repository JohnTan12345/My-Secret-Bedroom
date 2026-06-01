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
    private string tag = "Player";
    public UnityEvent ObjectEnterArea;
    public UnityEvent ObjectExitArea;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(transform.parent);
        if (other.CompareTag(tag))
        {
            ObjectEnterArea.Invoke();
        };
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tag))
        {
            ObjectExitArea.Invoke();
        };
    }
}
