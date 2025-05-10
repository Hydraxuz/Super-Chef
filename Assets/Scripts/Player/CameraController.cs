using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float minY = 10f; // Minimum Y position for the camera

    private Transform playerTransform;

    void Start()
    {
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            float targetY = Mathf.Max(playerTransform.position.y, minY);

            transform.position = new Vector3(
                playerTransform.position.x,
                targetY,
                transform.position.z
            );
        }
    }
}
