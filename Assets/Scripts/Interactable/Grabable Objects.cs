/*
    Created by: John
    Description: Moves the text UI from one spot to another
*/

using UnityEngine;

public class GrabableObjects : MonoBehaviour
{
    [Header("Text UI Positioning")]
    [SerializeField]
    private Transform TextUI;
    [SerializeField]
    private Transform grabbedPos;
    [SerializeField]
    private Transform originalPos;
    [Header("Object")]
    [SerializeField]
    private GameObject grabableObject;
    [SerializeField]
    private Vector3 originalObjPos;
    [SerializeField]
    private Quaternion originalObjRot;

    private bool grabbed = false;

    // Variable setup
    void Start()
    {
        // Set grabable object to current object if none is given
        if (grabableObject == null)
        {
            grabableObject = gameObject;
        }

        GameManager.instance.onGameReset.AddListener(Reset); 
        originalObjPos = grabableObject.transform.position;
        originalObjRot = grabableObject.transform.rotation;   
    }

    // Moves the text UI to new position that follows the object
    public void OnGrab()
    {
        if (grabbed) {return;}
        if (TextUI != null)
        {
            TextUI.position = grabbedPos.position;
            TextUI.rotation = grabbedPos.rotation;
            TextUI.SetParent(grabbedPos);
        }
        

        grabbed = true;
    }

    // Resets the text UI to original position and set the grabable object's velocity to 0
    private void Reset()
    {
        if (TextUI != null)
        {
            TextUI.position = originalPos.position;
            TextUI.rotation = originalPos.rotation;
            TextUI.SetParent(originalPos);
        }
        
        grabableObject.transform.position = originalObjPos;
        grabableObject.transform.rotation = originalObjRot;

        grabableObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        grabableObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        grabbed = false;
    }
}
