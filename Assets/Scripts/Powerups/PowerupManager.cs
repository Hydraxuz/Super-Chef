using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReactivatePowerup(GameObject powerup, float duration)
    {
        StartCoroutine(ReactivateAfterDelay(powerup, duration));
    }

    private IEnumerator ReactivateAfterDelay(GameObject powerup, float duration)
    {
        yield return new WaitForSeconds(duration);
        powerup.SetActive(true);
    }
}
