using UnityEngine;

public class ChangeBackgroundPiece : MonoBehaviour
{
    [Header("Set the background piece to change")]
    public SpriteRenderer backgroundPiece;

    [Header("New sprite to swap in when player enters")]
    public Sprite newBackgroundSprite;

    [Header("Tag of the player GameObject")]
    public string playerTag = "Player";

    private bool hasChanged = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasChanged && other.CompareTag(playerTag))
        {
            if (backgroundPiece != null && newBackgroundSprite != null)
            {
                backgroundPiece.sprite = newBackgroundSprite;
                hasChanged = true;
            }
            else
            {
                Debug.LogWarning("Background piece or new sprite not set!");
            }
        }
    }
}
