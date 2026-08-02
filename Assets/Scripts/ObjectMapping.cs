using UnityEngine;

public class ObjectMapping : MonoBehaviour
{
    public Vector3 OriginalPosition;
    public Quaternion OriginalRotation;

    void Awake()
    {
        OriginalPosition = transform.position;
        OriginalRotation = transform.rotation;
    }
}
