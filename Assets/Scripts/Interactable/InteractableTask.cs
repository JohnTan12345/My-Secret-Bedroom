/*
    Created by: John
    Description: Handles the task for the game
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTask : MonoBehaviour
{
    public int MaxProgress = 1;
    public GameObject ObjectHighlight;

    [Header("Settings")]
    [SerializeField]
    private bool automaticHighlighting = true;

    [Header("Hidden Parameters")]
    [SerializeField]
    private int currentProgress = 0;
    [SerializeField]
    private bool highlightingEnabled = false;
    [SerializeField]
    private bool objectHighlighted = false;
    [SerializeField]
    private List<MeshRenderer> highlightedObjectList = new();

    // Events
    [Header("Events")]
    [Space(5)]
    public UnityEvent onTaskStart; // Fires when the task starts
    public UnityEvent onTaskComplete; // Fires when the task is complete

    void Awake()
    {   
        // Check if the progress is set up correctly
        if (MaxProgress <= 0)
        {
            throw new System.Exception("Max Progress cannot be 0 or less");
        }

        StartCoroutine(WaitForGameManagerInstance());
    }

    private IEnumerator WaitForGameManagerInstance()
    {
        // Wait for the game manager to load before subscribing to onGameReset
        yield return new WaitUntil(() => GameManager.instance != null);
        GameManager.instance.onGameReset.AddListener(ResetProgress);

        HighlightObjectSetUp();
    }

    public void AddProgress(int amount)
    {
        currentProgress += amount; // Adds to the current progress the given amount

        if (currentProgress >= MaxProgress) // Check if current progress is more than the max progress
        {
            onTaskComplete.Invoke(); // Fires the task complete event
            ResetProgress(); // Resets the progress (may be removed)
        }
    }
    public void ResetProgress()
    {
        currentProgress = 0; // Resets progress to 0
        HighlightObject(false);
    }

    private void HighlightObjectSetUp()
    {
        // Check if theres an object to be highlighted or the highlight material is assigned
        if (ObjectHighlight == null || GameManager.instance.highlightMaterial == null)
        {
            if (ObjectHighlight == null)
            {
                Debug.LogWarning("No objects given to highlight");
            }
            if (GameManager.instance.highlightMaterial == null)
            {
                Debug.LogError("Highlight material is not assigned in GameManager");
            }

            return;
        }

        highlightingEnabled = true;
        ObjectHighlight.TryGetComponent(out MeshRenderer parentMeshRenderer);

        // Add highlighted object's mesh renderer into highlight list
        if (parentMeshRenderer != null)
        {
            highlightedObjectList.Add(parentMeshRenderer);
        }

        // Add all children's mesh renderer into highlight list
        if (ObjectHighlight.transform.childCount > 0)
            {
                for (int i = 0; i < ObjectHighlight.transform.childCount; i++)
                {
                    if (!ObjectHighlight.transform.GetChild(i).gameObject.activeSelf)
                    {
                        continue;
                    }
                    ObjectHighlight.transform.GetChild(i).TryGetComponent(out MeshRenderer meshRenderer);

                    if (meshRenderer != null)
                    {
                        highlightedObjectList.Add(meshRenderer);
                    }
                }
            }

        // Automatically highlight object when the task start / unhighlight object when the task end
        if (automaticHighlighting)
        {
            onTaskStart.AddListener(() => HighlightObject(true));
            onTaskComplete.AddListener(() => HighlightObject(false));
        }
        
    }

    public void HighlightObject(bool val)
    {
        // If highlighting is disabled or given value is the same
        if (!highlightingEnabled || objectHighlighted == val)
        {
            return;
        }

        if (val)
        {
            // Highlights the object and its children
            objectHighlighted = true;
            foreach (MeshRenderer meshRenderer in highlightedObjectList)
            {
                List<Material> materials = new();
                meshRenderer.GetMaterials(materials);
                materials.Add(GameManager.instance.highlightMaterial);
                meshRenderer.SetMaterials(materials);
            }
        }
        else
        {
            // Unhighlights the object and its children
            objectHighlighted = false;
            foreach (MeshRenderer meshRenderer in highlightedObjectList)
            {
                List<Material> materials = new();
                meshRenderer.GetMaterials(materials);
                materials.RemoveAt(1);
                meshRenderer.SetMaterials(materials);
            }
        }
    }
}
