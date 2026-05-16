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

        Debug.DrawRay(PlayerLookObject.transform.position, PlayerLookObject.transform.forward * maxDistance, Color.green);
        if (Physics.Raycast(PlayerLookObject.transform.position, PlayerLookObject.transform.forward, out RaycastHit hitInfo, maxDistance, LayerMask.GetMask("Player Look Area"), QueryTriggerInteraction.Collide))
        {
            hitInfo.collider.GetComponent<LookArea>().OnLook();
        }
        
    }
}
