using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class TransparencyOnTrigger : MonoBehaviour
{
    public GameObject objectToMakeTransparent; // The object to make transparent
    public float transparencyLevel = 1f; // The transparency level (0 = fully transparent, 1 = fully opaque)
    private Color color;
    private float a = 0;
    public float spd;
    private void Start()
    {
        // Ensure the object has a Renderer component
        if (objectToMakeTransparent != null && objectToMakeTransparent.GetComponent<SpriteRenderer>() != null)
        {
            color = objectToMakeTransparent.GetComponent<SpriteRenderer>().color;
            // Create a new material with the same properties as the original
            color.a = 0;

            objectToMakeTransparent.GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            Debug.LogError("Object to make transparent is missing or does not have a Renderer component.");
        }
    }
   void Update(){
        color.a += Mathf.Clamp(a-color.a,-Time.deltaTime*spd,Time.deltaTime*spd);
        objectToMakeTransparent.GetComponent<SpriteRenderer>().color = color;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Exit");
        // Check if the player enters the trigger
        if (other.gameObject.CompareTag("Player"))
        {
            a=0;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Enter");
        // Check if the player exits the trigger
        if (other.gameObject.CompareTag("Player"))
        {
            a=1;
        }
    }
}