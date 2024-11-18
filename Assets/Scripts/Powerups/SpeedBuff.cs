using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class SpeedBuff : PowerupEffect
{
    public float amount;
    public float duration; // Duration for how long the speed buff lasts

    public override void Apply(GameObject target)
    {
        PlayerController playerController = target.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Cache components to improve performance
            SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();

            // Store the original color for resetting
            Color originalColor = spriteRenderer.color;

            // Apply speed buff and change color to yellow
            playerController.speed += amount;
            spriteRenderer.color = Color.yellow;

            // Coroutine to remove the buff after the specified duration
            target.GetComponent<MonoBehaviour>().StartCoroutine(RemoveBuffAfterDuration(playerController, spriteRenderer, originalColor));
        }
    }

    private IEnumerator RemoveBuffAfterDuration(PlayerController playerController, SpriteRenderer spriteRenderer, Color originalColor)
    {
        yield return new WaitForSeconds(duration);
        playerController.speed = 12;  // Set speed to 12 directly

        // Reset color to original
        spriteRenderer.color = originalColor;
    }
}
