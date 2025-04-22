using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransparencyOnTrigger : MonoBehaviour
{
    public GameObject objectToMakeTransparent; // The object to make transparent
    public float targetTransparency = 0.5f; // The transparency level to fade to (0 = fully transparent, 1 = fully opaque)
    private Color color;
    private float originalTransparency; // Store the original transparency before being touched
    public float spd; // Speed of the fade effect
    private float targetAlpha; // Target alpha (transparency value) for fading

    private void Start()
    {
        // Ensure the object has a Renderer component
        if (objectToMakeTransparent != null && objectToMakeTransparent.GetComponent<SpriteRenderer>() != null)
        {
            color = objectToMakeTransparent.GetComponent<SpriteRenderer>().color;
            // Store the original transparency level
            originalTransparency = color.a;

            // Set the initial color alpha to original transparency
            color.a = originalTransparency;
            objectToMakeTransparent.GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            Debug.LogError("Object to make transparent is missing or does not have a Renderer component.");
        }
    }

    void Update()
    {
        // Smoothly transition the alpha value to the target transparency (targetAlpha)
        color.a += Mathf.Clamp(targetAlpha - color.a, -Time.deltaTime * spd, Time.deltaTime * spd);
        objectToMakeTransparent.GetComponent<SpriteRenderer>().color = color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When the player enters the trigger area, fade to the target transparency
        if (other.gameObject.CompareTag("Player"))
        {
            targetAlpha = targetTransparency;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // When the player exits the trigger area, restore the original transparency
        if (other.gameObject.CompareTag("Player"))
        {
            targetAlpha = originalTransparency;
        }
    }
}
