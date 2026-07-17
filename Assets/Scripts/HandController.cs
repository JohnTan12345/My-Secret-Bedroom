/*
    Created by: Xander
    Modified by: John
    Description: Manages the hands
*/
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    [Header("Hand Animator")]
    public Animator handAnimator;

    private void OnEnable()
    {
        pinchAnimationAction.action.performed += TriggerToggle;
        pinchAnimationAction.action.canceled += TriggerToggle;
        gripAnimationAction.action.performed += GripToggle;
        gripAnimationAction.action.canceled += GripToggle;
    }

    private void OnDisable()
    {
        pinchAnimationAction.action.performed -= TriggerToggle;
        pinchAnimationAction.action.canceled -= TriggerToggle;
        gripAnimationAction.action.performed -= GripToggle;
        gripAnimationAction.action.canceled -= GripToggle;
    }

    private void TriggerToggle(InputAction.CallbackContext action)
    {
        handAnimator.SetFloat("Trigger", action.ReadValue<float>());
    }

    private void GripToggle(InputAction.CallbackContext action)
    {
        handAnimator.SetFloat("Grip", action.ReadValue<float>());
    }

}