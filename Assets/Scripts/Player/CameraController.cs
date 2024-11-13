using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float heightOffset = 5.0f;

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
            transform.position = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + heightOffset,
                transform.position.z
            );
        }
    }
}
