using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class TransparencyOnTrigger : MonoBehaviour
{
    public GameObject objectToMakeTransparent; // The object to make transparent
    public float transparencyLevel = 0.5f; // The transparency level (0 = fully transparent, 1 = fully opaque)

    private Material originalMaterial; // Store the original material
    private Material transparentMaterial; // Store the transparent material

    private void Start()
    {
        // Ensure the object has a Renderer component
        if (objectToMakeTransparent != null && objectToMakeTransparent.GetComponent<Renderer>() != null)
        {
            // Store the original material
            originalMaterial = objectToMakeTransparent.GetComponent<Renderer>().material;

            // Create a new material with the same properties as the original
            transparentMaterial = new Material(originalMaterial);

            // Set the transparency level
            Color color = transparentMaterial.color;
            color.a = transparencyLevel;
            transparentMaterial.color = color;
        }
        else
        {
            Debug.LogError("Object to make transparent is missing or does not have a Renderer component.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            // Change the object's material to the transparent one
            if (objectToMakeTransparent != null && transparentMaterial != null)
            {
                objectToMakeTransparent.GetComponent<Renderer>().material = transparentMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger
        if (other.CompareTag("Player"))
        {
            // Revert the object's material back to the original
            if (objectToMakeTransparent != null && originalMaterial != null)
            {
                objectToMakeTransparent.GetComponent<Renderer>().material = originalMaterial;
            }
        }
    }
}