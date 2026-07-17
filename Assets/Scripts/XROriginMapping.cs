/*
    Created by: John
    Description: XR Origin mapping for settings
*/

using UnityEngine;

public class XROriginMapping : MonoBehaviour
{
    public static XROriginMapping instance;

    public GameObject MoveLocomotion;
    public GameObject TeleportLocomotion;
    public GameObject CameraOffset;

    // Force any setting changes to use this script
    void Awake()
    {
        instance = this;
    }
}
