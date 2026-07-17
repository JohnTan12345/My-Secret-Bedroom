/*
    Created by: Xander
    Description: Controls when SFX are played and stopped
*/
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField]
    private AudioSource areaSFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            areaSFX.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            areaSFX.Stop();
        }
    }
}
