using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    public GameObject fallDetector;

    private static List<GameObject> deactivatedEnemies = new List<GameObject>();
    private float originalSpeed = 12f;
    private Color originalColor = Color.white;

    private void Start()
    {
        respawnPoint = transform.position; // Initialize respawn point
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null) originalSpeed = playerController.speed;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    private void Update()
    {
        UpdateFallDetectorPosition();
    }

    private void UpdateFallDetectorPosition()
    {
        if (fallDetector != null)
            fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallDetector"))
        {
            Respawn();
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            respawnPoint = transform.position; // Update respawn point
        }
        else if (collision.CompareTag("NextLevel"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position; // Update respawn point
        }
    }

    public void Respawn()
    {
        if (!gameObject.activeInHierarchy) return; // Check if the player is active

        transform.position = respawnPoint;
        ResetPlayerState();

        foreach (var enemy in deactivatedEnemies)
        {
            enemy.SetActive(true);
        }
        deactivatedEnemies.Clear();

        gameObject.SetActive(true);
    }

    private void ResetPlayerState()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.speed = originalSpeed; // Restore original speed
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = originalColor; // Restore original color
        }
    }

    public static void RegisterDeactivatedEnemy(GameObject enemy)
    {
        if (!deactivatedEnemies.Contains(enemy))
        {
            deactivatedEnemies.Add(enemy);
        }
    }
}
