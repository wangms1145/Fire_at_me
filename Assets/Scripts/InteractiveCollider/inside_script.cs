using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TransparencyOnTrigger : MonoBehaviour
{
    public GameObject objectToMakeTransparent; // The object to make transparent
    public float transparencyLevel = 0.5f; // The transparency level (0 = fully transparent, 1 = fully opaque)
    private Color originalColor;
    private Color transparentColor;
    private void Start()
    {
        // Ensure the object has a Renderer component
        if (objectToMakeTransparent != null && objectToMakeTransparent.GetComponent<SpriteRenderer>() != null)
        {
            // Store the original material
            originalColor = objectToMakeTransparent.GetComponent<SpriteRenderer>().color;
            transparentColor = new Color(originalColor.r, originalColor.g,originalColor.b);
            // Create a new material with the same properties as the original
            

            // Set the transparency level
            originalColor.a = transparencyLevel;
            transparentColor.a = 0;
            objectToMakeTransparent.GetComponent<SpriteRenderer>().color = transparentColor;
        }
        else
        {
            Debug.LogError("Object to make transparent is missing or does not have a Renderer component.");
        }
    }
    void Update(){

    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit");
        // Check if the player enters the trigger
        if (other.gameObject.CompareTag("Player"))
        {
            // Change the object's material to the transparent one
            if (objectToMakeTransparent != null && transparentColor != null)
            {
                objectToMakeTransparent.GetComponent<SpriteRenderer>().color = transparentColor;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        // Check if the player exits the trigger
        if (other.gameObject.CompareTag("Player"))
        {
            // Revert the object's material back to the original
            if (objectToMakeTransparent != null && originalColor != null)
            {
                objectToMakeTransparent.GetComponent<SpriteRenderer>().color = originalColor;
            }
        }
    }
}