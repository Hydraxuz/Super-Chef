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
            Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

            // Store original speed so we can restore it
            float originalSpeed = playerController.speed;

            // Apply speed buff and change color to yellow
            playerController.speed = originalSpeed + amount;
            if (spriteRenderer != null) spriteRenderer.color = Color.yellow;

            // Request the PowerupManager to run the coroutine; fallback to starting on target if manager missing
            var routine = RemoveBuffAfterDuration(playerController, spriteRenderer, originalColor, originalSpeed);
            if (PowerupManager.Instance != null)
                PowerupManager.Instance.RunCoroutine(routine);
            else
            {
                var mb = target.GetComponent<MonoBehaviour>();
                if (mb != null)
                    mb.StartCoroutine(routine);
                else
                    Debug.LogWarning("Cannot start powerup coroutine: no PowerupManager and target has no MonoBehaviour to run coroutines.");
            }
        }
    }
    private IEnumerator RemoveBuffAfterDuration(PlayerController playerController, SpriteRenderer spriteRenderer, Color originalColor, float originalSpeed)
    {
        yield return new WaitForSeconds(duration);
        if (playerController != null)
            playerController.speed = originalSpeed;

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }
}
