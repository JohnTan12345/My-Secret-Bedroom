using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    [Header("Hand Animator")]
    public Animator handAnimator;

    private void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}