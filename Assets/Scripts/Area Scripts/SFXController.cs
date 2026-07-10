/*
    Created by: Xander
    Description: Controls when SFX are played and stopped
*/
using UnityEngine;

public class CorridorSFXController : MonoBehaviour
{
    [SerializeField]
    private AudioSource corridorSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            corridorSFX.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            corridorSFX.Stop();
        }
    }
}
