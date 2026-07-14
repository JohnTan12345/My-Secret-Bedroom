using UnityEngine;

public class XROriginMapping : MonoBehaviour
{
    public static XROriginMapping instance;

    public GameObject MoveLocomotion;
    public GameObject TeleportLocomotion;
    public GameObject CameraOffset;

    void Awake()
    {
        instance = this;
    }
}
