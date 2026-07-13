using UnityEngine;

public class XROriginMapping : MonoBehaviour
{
    public static XROriginMapping instance;

    public GameObject MoveLocomotion;
    public GameObject TeleportLocomotion;

    void Awake()
    {
        instance = this;
    }
}
