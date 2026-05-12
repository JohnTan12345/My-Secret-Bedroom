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
