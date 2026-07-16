/*
    Created by: John
    Description: Currently manages where players look
*/

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerLookObject;
    [SerializeField]
    private int maxDistance = 5;

    void Awake()
    {
        if (PlayerLookObject == null)
        {
            PlayerLookObject = gameObject;
        }
    }
    void FixedUpdate()
    {
        // Shoot a ray that checks for "LookArea"s
        bool firstColliderCheck = false;
        Debug.DrawRay(PlayerLookObject.transform.position, PlayerLookObject.transform.forward * maxDistance, Color.green);
        if (Physics.Raycast(PlayerLookObject.transform.position, PlayerLookObject.transform.forward, out RaycastHit hitInfo, maxDistance, LayerMask.GetMask("Player Look Area"), QueryTriggerInteraction.Collide))
        {
            // If the ray hits a Look Area
            if (!firstColliderCheck)
            {
                firstColliderCheck = true;
                hitInfo.collider.GetComponent<LookArea>().OnLook();
            }
            
        }

    }
}
