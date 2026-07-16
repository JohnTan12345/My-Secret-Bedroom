/*
    Created by: John
    Description: Detects the rotation of the head and returns a value.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadMovementCheck : MonoBehaviour
{
    public static HeadMovementCheck instance;
    [SerializeField]
    private int nodAngleThreshold;
    [SerializeField]
    private int shakeAngleThreshold;
    [SerializeField]
    private int maxCheck = 3;
    [SerializeField]
    private Transform head;

    public UnityEvent<DetectionResult> onDetectionFinish;

    private bool checkActive = false;

    [Header("Hidden Parameters")]
    [SerializeField]
    private bool debuggingEnabled = false;
    [SerializeField]
    private bool advancedDebuggingEnabled = false;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;

       if (head == null) // If head is not given
        {
            head = transform;
        }

        if (shakeAngleThreshold < 0) // Account for negative value
        {
            shakeAngleThreshold = System.Math.Abs(shakeAngleThreshold); 
        }
    }

    void Start()
    {
        GameManager.instance.onGameReset.AddListener(() => StopDetection("Game Resetted")); // Stop detection if the game resetted

    }

    public void StartHeadDetection()
    {
        if (debuggingEnabled)
        {
            Debug.Log("Head detection started");
        }
        StartCoroutine(HeadMovementDetection()); // Start the head detection
    }

    private IEnumerator HeadMovementDetection()
    {
        checkActive = true;

        DetectionResult result = new();
        Dictionary<string, bool> faceChecks = new(4) { { "left", false }, {"right", false}, {"up", false}, {"down", false} };

        // Variable setup before starting the check
        int nodCount = 0;
        int shakeCount = 0;

        float prevX = head.eulerAngles.x;
        float prevY = head.eulerAngles.y;

        float headXThresholdMin = 180 - nodAngleThreshold;
        float headXThresholdMax = 180 + nodAngleThreshold;

        float headYThresholdMin = 180 - shakeAngleThreshold;
        float headYThresholdMax = 180 + shakeAngleThreshold;

        // Current head position
        float referenceX = 180;
        float referenceY = 180;

        while (checkActive)
        {
            // Stores the original position before overflow/underflow check
            float originalX = head.eulerAngles.x;
            float originalY = head.eulerAngles.y;
            
            // Stores final position after overflow/underflow check
            float finalX = originalX;
            float finalY = originalY;

            // Check if the new X went over 360 or went under 0
            if (Mathf.Abs(prevX - originalX) > 300f)
            {
                if (originalX < 180) // if x overflows 360
                {
                    finalX += 360;
                }
                else // if x underflows 360
                {
                    finalX -= 360;
                }
            }

            // Check if the new Y went over 360 or went under 0
            if (Mathf.Abs(prevY - originalY) > 300f)
            {
                if (originalY < 180) // if y overflows 360
                {
                    finalY += 360;
                }
                else // if y underflows 360
                {
                    finalY -= 360;
                }
            }

            // Add the change to the reference
            referenceX += prevX - finalX;
            referenceY += prevY - finalY;

            // Set the new pevious
            prevX = originalX;
            prevY = originalY;

            // Nod detection based on reference position
            if (referenceX < headXThresholdMin && !faceChecks["up"]) // // Check if head is facing up from reference
            {
                nodCount++;
                shakeCount = 0;
                faceChecks["up"] = true;
                faceChecks["down"] = false;
            }
            else if (referenceX > headXThresholdMax && !faceChecks["down"]) // Check if head is facing down from reference
            {
                nodCount++;
                shakeCount = 0;
                faceChecks["down"] = true;
                faceChecks["up"] = false;
            }

            // Shake detection based on reference position
            if (referenceY < headYThresholdMin && !faceChecks["left"]) // Check if head is facing left from reference
            {
                shakeCount++;
                nodCount = 0;
                faceChecks["left"] = true;
                faceChecks["right"] = false;
            }
            else if (referenceY > headYThresholdMax && !faceChecks["right"]) // Check if head is facing right from reference
            {
                shakeCount++;
                nodCount = 0;
                faceChecks["right"] = true;
                faceChecks["left"] = false; 
            }

            // Once the check reaches the threshold
            if (nodCount >= maxCheck || shakeCount >= maxCheck)
            {
                result.nodding = nodCount >= maxCheck;
                result.shaking = shakeCount >= maxCheck;
                checkActive = false;
                break; // Stop the check
            }

            // Debugging
            if (debuggingEnabled && advancedDebuggingEnabled)
            {
                Debug.Log($"head angle: x: {originalX}, y: {originalY}; thresholds: x: [ min: {headXThresholdMin}, max: {headXThresholdMax} ], y: [ min: {headYThresholdMin}, max: {headYThresholdMax} ]");
                Debug.Log($"Recorded Reference: x: {referenceX} y: {referenceY}");
                Debug.Log($"detection: [ up: {referenceX < headXThresholdMin}, down: {referenceX > headXThresholdMax}, left: {referenceY < headYThresholdMin}, right: {referenceY > headYThresholdMax}]");
                Debug.Log($"Counters: Nod Count: {nodCount}, Shake Count: {shakeCount}, Checks: [Left: {faceChecks["left"]}, Right: {faceChecks["right"]}, Up: {faceChecks["up"]}, Down: {faceChecks["down"]}]");
            }
            yield return new WaitForFixedUpdate();
        }

        if (debuggingEnabled)
        {
            Debug.Log("Head detection ended");
        }

        onDetectionFinish.Invoke(result);
    }

    // Stop detection with a reason
    public void StopDetection(string msg)
    {
        Debug.LogWarning($"Stopped due to the following reason: {msg}");
        checkActive = false;
    }
}

public class DetectionResult
{
    public bool nodding = false;
    public bool shaking = false;
}