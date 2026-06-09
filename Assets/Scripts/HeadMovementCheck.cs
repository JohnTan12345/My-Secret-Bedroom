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
    private float detectionDuration = 0.5f;
    [SerializeField]
    private Transform head;

    public UnityEvent<DetectionResult> onDetectionFinish;

    private Quaternion headRotation;

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

        StartCoroutine(HeadMovementDetection());
    }

    public IEnumerator HeadMovementDetection()
    {
        DetectionResult result = new();

        headRotation = head.rotation;

        Dictionary<string, bool> faceChecks = new() { { "left", false }, {"right", false}, {"up", false}, {"down", false} };

        int nodCount = 0;
        int shakeCount = 0;

        while (true)
        {
            float headX = head.eulerAngles.x < 180 ? head.eulerAngles.x + 180 : head.eulerAngles.x - 180;
            float headXThresholdMin = headRotation.eulerAngles.x - nodAngleThreshold + 180;
            float headXThresholdMax = headRotation.eulerAngles.x + nodAngleThreshold + 180;

            float headY = head.eulerAngles.y < 180 ? head.eulerAngles.y + 180 : head.eulerAngles.y - 180;
            float headYThresholdMin = headRotation.eulerAngles.y - shakeAngleThreshold + 180;
            float headYThresholdMax = headRotation.eulerAngles.y + shakeAngleThreshold + 180;


            Debug.Log($"head angle: x: {headX}, y: {headY}; thresholds: x: [ min: {headXThresholdMin}, max: {headXThresholdMax} ], y: [ min: {headYThresholdMin}, max: {headYThresholdMax} ]");
            Debug.Log($"detection: [ up: {headX < headXThresholdMin}, down: {headX > headXThresholdMax}, left: {headY < headYThresholdMin}, right: {headY > headYThresholdMax}]");

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

            Debug.Log($"Counters: Nod Count: {nodCount}, Shake Count: {shakeCount}, Checks: [Left: {faceChecks["left"]}, Right: {faceChecks["right"]}, Up: {faceChecks["up"]}, Down: {faceChecks["down"]}]");
            yield return new WaitForFixedUpdate();
        }

        onDetectionFinish.Invoke(result);
    }
}

public class DetectionResult
{
    public bool nodding = false;
    public bool shaking = false;
}