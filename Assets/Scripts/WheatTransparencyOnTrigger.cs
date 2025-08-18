using UnityEngine;

public class TransparencyOnTouch : MonoBehaviour
{
    public GameObject wheatObject; // The Wheat object that will change transparency
    public float targetTransparency = 0.5f; // The transparency level when player touches the object (0 = fully transparent, 1 = fully opaque)
    private float originalTransparency; // Store the original transparency level of the Wheat object
    private SpriteRenderer wheatRenderer; // Reference to the Wheat's SpriteRenderer
    private float currentAlpha; // Current alpha value that is being modified
    public float fadeSpeed = 1f; // The speed at which the fade effect happens

    void Start()
    {
        // Get the SpriteRenderer component of the Wheat object
        if (wheatObject != null)
        {
            wheatRenderer = wheatObject.GetComponent<SpriteRenderer>();
            if (wheatRenderer != null)
            {
                originalTransparency = wheatRenderer.color.a; // Store the initial transparency of the Wheat
                currentAlpha = originalTransparency; // Start with the original transparency
            }
            else
            {
                Debug.LogError("Wheat object does not have a SpriteRenderer component.");
            }
        }
        else
        {
            Debug.LogError("Wheat object is not assigned.");
        }
    }

    void Update()
    {
        // Update the alpha value of the Wheat object to smoothly transition to the target transparency
        Color color = wheatRenderer.color;
        color.a = Mathf.Lerp(color.a, currentAlpha, Time.deltaTime * fadeSpeed);
        wheatRenderer.color = color; // Apply the updated color to the Wheat object
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters the trigger zone, set the target transparency to the specified value
        if (other.CompareTag("Player"))
        {
            currentAlpha = targetTransparency; // Fade to the target transparency
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If the player exits the trigger zone, restore the original transparency
        if (other.CompareTag("Player"))
        {
            currentAlpha = originalTransparency; // Fade back to the original transparency
        }
    }
}
