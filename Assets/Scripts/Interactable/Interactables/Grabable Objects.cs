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

    void Start()
    {

        if (grabableObject == null)
        {
            grabableObject = gameObject;
        }

        GameManager.instance.onGameReset.AddListener(Reset); 
        originalObjPos = grabableObject.transform.position;
        originalObjRot = grabableObject.transform.rotation;   
    }

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

        grabbed = false;
    }
}
