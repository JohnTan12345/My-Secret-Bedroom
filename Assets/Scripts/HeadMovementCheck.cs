using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadMovementCheck : MonoBehaviour
{
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

    [SerializeField]
    private bool debuggingEnabled = false;
    [SerializeField]
    private bool advancedDebuggingEnabled = false;
    // 
    void Awake()
    {
       if (head == null)
        {
            head = transform;
        }

        if (shakeAngleThreshold < 0)
        {
            shakeAngleThreshold = System.Math.Abs(shakeAngleThreshold); 
        }
    }

    void Start()
    {
        GameManager.instance.onGameReset.AddListener(() => StopDetection("Game Resetted"));
    }

    public void StartHeadDetection()
    {
        if (debuggingEnabled)
        {
            Debug.Log("Head detection started");
        }
        StartCoroutine(HeadMovementDetection());
    }

    private IEnumerator HeadMovementDetection()
    {
        checkActive = true;

        DetectionResult result = new();
        Dictionary<string, bool> faceChecks = new() { { "left", false }, {"right", false}, {"up", false}, {"down", false} };

        int nodCount = 0;
        int shakeCount = 0;

        float headXThresholdMin = 180 - nodAngleThreshold;
        float headXThresholdMax = 180 + nodAngleThreshold;

        float headYThresholdMin = 180 - shakeAngleThreshold;
        float headYThresholdMax = 180 + shakeAngleThreshold;

        float prevX = head.eulerAngles.x;
        float prevY = head.eulerAngles.y;

        float referenceX = 180;
        float referenceY = 180;

        while (checkActive)
        {
            float originalX = head.eulerAngles.x;
            float originalY = head.eulerAngles.y;

            float finalX = originalX;
            float finalY = originalY;

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

            referenceX += prevX - finalX;
            referenceY += prevY - finalY;

            prevX = originalX;
            prevY = originalY;

            // Nod detection
            if (referenceX < headXThresholdMin && !faceChecks["up"])
            {
                nodCount++;
                shakeCount = 0;
                faceChecks["up"] = true;
                faceChecks["down"] = false;
            }
            else if (referenceX > headXThresholdMax && !faceChecks["down"])
            {
                nodCount++;
                shakeCount = 0;
                faceChecks["down"] = true;
                faceChecks["up"] = false;
            }

            // Shake detection
            if (referenceY < headYThresholdMin && !faceChecks["left"])
            {
                shakeCount++;
                nodCount = 0;
                faceChecks["left"] = true;
                faceChecks["right"] = false;
            }
            else if (referenceY > headYThresholdMax && !faceChecks["right"])
            {
                shakeCount++;
                nodCount = 0;
                faceChecks["right"] = true;
                faceChecks["left"] = false; 
            }

            if (nodCount >= maxCheck || shakeCount >= maxCheck)
            {
                result.nodding = nodCount >= maxCheck;
                result.shaking = shakeCount >= maxCheck;
                checkActive = false;
                break;
            }
    
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