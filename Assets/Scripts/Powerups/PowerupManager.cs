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
            DontDestroyOnLoad(gameObject);
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

    /// <summary>
    /// Allow ScriptableObjects to request coroutines run on the manager.
    /// </summary>
    public Coroutine RunCoroutine(IEnumerator routine)
    {
        if (routine == null || !enabled) return null;
        return StartCoroutine(routine);
    }

    private IEnumerator ReactivateAfterDelay(GameObject powerup, float duration)
    {
        yield return new WaitForSeconds(duration);
        powerup.SetActive(true);
    }
}

