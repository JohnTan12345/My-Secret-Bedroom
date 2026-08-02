using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectArea : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        bool success = other.TryGetComponent<XRBaseInteractable>(out _);
        if (!success)
        {
            Debug.Log("AAA");
            return;
        }

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb.isKinematic)
        {
            return;
        }

        success = other.TryGetComponent(out ObjectMapping mapping);

        if (!success)
        {
            Debug.LogError($"{other.gameObject.name} does not have ObjectMapping script attached. Please add the ObjectMapping script to it for the object to return to it's original position.");
            return;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        other.transform.position = mapping.OriginalPosition;
        other.transform.rotation = mapping.OriginalRotation;
    }
}
