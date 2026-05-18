using UnityEngine;

public class GrabableObjects : MonoBehaviour
{
    [SerializeField]
    private Transform TextUI;
    [SerializeField]
    private Transform grabbedPos;
    [SerializeField]
    private Transform originalPos;
    [SerializeField]
    private Vector3 originalObjPos;
    [SerializeField]
    private Quaternion originalObjRot;

    private bool grabbed = false;

    void Start()
    {
        GameManager.instance.onGameReset.AddListener(Reset); 
        originalObjPos = transform.position;
        originalObjRot = transform.rotation;   
    }

    public void OnGrab()
    {
        if (grabbed) {return;}

        TextUI.position = grabbedPos.position;
        TextUI.rotation = grabbedPos.rotation;
        TextUI.SetParent(grabbedPos);

        grabbed = true;
    }

    private void Reset()
    {
        TextUI.position = originalPos.position;
        TextUI.rotation = originalPos.rotation;
        TextUI.SetParent(originalPos);
        transform.position = originalObjPos;
        transform.rotation = originalObjRot;

        grabbed = false;
    }
}
