using UnityEngine;

public class BlackHoleShakeAndSpin : MonoBehaviour
{
    // Public variables for shaking
    public float shakeIntensity = 0.1f;

    // Public variables for spinning
    public float spinSpeed = 50f; // Speed of rotation (degrees per second)
    public Vector3 spinAxis = Vector3.up; // Axis to spin around (default is Y-axis)

    // Original position of the GameObject
    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the GameObject
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // Shake effect
        Shake();

        // Spin effect
        //Spin();
    }

    void Shake()
    {
        // Generate a random offset based on the shake intensity
        float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
        float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
        float offsetZ = Random.Range(-shakeIntensity, shakeIntensity);

        // Apply the offset to the original position
        transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, offsetZ);
    }

    void Spin()
    {
        // Rotate the GameObject around the specified axis at the given speed
        transform.Rotate(spinAxis.normalized, spinSpeed * Time.deltaTime);
    }
}