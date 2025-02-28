using UnityEngine;

public class MapGravity2D : MonoBehaviour
{
    // Public array to assign all space rocks (gravity sources)
    public GravitySource2D[] gravitySources;

    void FixedUpdate()
    {
        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        // Apply gravity from all sources
        foreach (var source in gravitySources)
        {
            if (source.sourceTransform != null)
            {
                ApplyGravity(playerRb, source.sourceTransform, source.gravityStrength, source.minDistance);
            }
        }
    }

    void ApplyGravity(Rigidbody2D rb, Transform target, float gravityStrength, float minDistance)
    {
        // Calculate direction from the player to the gravity source
        Vector2 direction = (target.position - rb.transform.position);

        // Calculate distance to the gravity source
        float distance = direction.magnitude;

        // Apply gravity only if beyond the minimum distance
        if (distance > minDistance)
        {
            direction.Normalize(); // Get the direction unit vector

            // Calculate gravitational force and apply it
            Vector2 gravityForce = direction * gravityStrength * Time.fixedDeltaTime;
            rb.AddForce(gravityForce);
        }
    }
}

[System.Serializable]
public class GravitySource2D
{
    public Transform sourceTransform; // Space rock (gravity source)
    public float gravityStrength = 9.81f; // Gravity pull strength
    public float minDistance = 1f; // Minimum distance for gravity effect
}

e4x3

//2