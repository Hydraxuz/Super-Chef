using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    public float inactiveDuration = 5f; // Duration to keep the object inactive

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.CompareTag("Player"))
        {
            if (powerupEffect != null)
                powerupEffect.Apply(collision.gameObject);

            if (PowerupManager.Instance != null)
            {
                PowerupManager.Instance.ReactivatePowerup(gameObject, inactiveDuration);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("PowerupManager.Instance is null — powerup will not be reactivated automatically.");
                gameObject.SetActive(false);
            }
        }
    }
}
