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

    private Quaternion headRotation;
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
        DetectionResult result = new();

        headRotation = head.rotation;

        Dictionary<string, bool> faceChecks = new() { { "left", false }, {"right", false}, {"up", false}, {"down", false} };

        int nodCount = 0;
        int shakeCount = 0;
        checkActive = true;
        while (checkActive)
        {

            float referenceX = headRotation.eulerAngles.x < 180 ? headRotation.eulerAngles.x + 180 : headRotation.eulerAngles.x - 180;
            float referenceY = headRotation.eulerAngles.y < 180 ? headRotation.eulerAngles.y + 180 : headRotation.eulerAngles.y - 180;

            float headX = head.eulerAngles.x < 180 ? head.eulerAngles.x + 180 : head.eulerAngles.x - 180;
            float headXThresholdMin = referenceX - nodAngleThreshold;
            float headXThresholdMax = referenceX + nodAngleThreshold;

            float headY = head.eulerAngles.y < 180 ? head.eulerAngles.y + 180 : head.eulerAngles.y - 180;
            float headYThresholdMin = referenceY - shakeAngleThreshold;
            float headYThresholdMax = referenceY + shakeAngleThreshold;

            // Nod detection
            if (headX < headXThresholdMin && !faceChecks["up"])
            {
                nodCount++;
                shakeCount = 0;
                faceChecks["up"] = true;
                faceChecks["down"] = false;
            }
            else if (headX > headXThresholdMax && !faceChecks["down"])
            {
                nodCount++;
                shakeCount = 0;
                faceChecks["down"] = true;
                faceChecks["up"] = false;
            }

            // Shake detection
            if (headY < headYThresholdMin && !faceChecks["left"])
            {
                shakeCount++;
                nodCount = 0;
                faceChecks["left"] = true;
                faceChecks["right"] = false;
            }
            else if (headY > headYThresholdMax && !faceChecks["right"])
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
                Debug.Log($"head angle: x: {headX}, y: {headY}; thresholds: x: [ min: {headXThresholdMin}, max: {headXThresholdMax} ], y: [ min: {headYThresholdMin}, max: {headYThresholdMax} ]");
                Debug.Log($"Reference Point: x: {referenceX}, y: {referenceY}");
                Debug.Log($"detection: [ up: {headX < headXThresholdMin}, down: {headX > headXThresholdMax}, left: {headY < headYThresholdMin}, right: {headY > headYThresholdMax}]");
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

    private void StopDetection(string msg)
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