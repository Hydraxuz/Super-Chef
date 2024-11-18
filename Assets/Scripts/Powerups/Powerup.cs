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
            powerupEffect.Apply(collision.gameObject);
            PowerupManager.Instance.ReactivatePowerup(gameObject, inactiveDuration);
            gameObject.SetActive(false);
        }
    }
}
