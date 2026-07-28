using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public Transform player; 
    public float speed = 2f;
    public bool verticalMovement = false; 

    [Range(-10f, 0)] public float minBound = -10f; 
    [Range(0, 10f)] public float maxBound = 10f;  
    
    [Range (15f, 100f)] public float bounceForce = 15f;
    private Rigidbody2D rb;
    private bool movingForward = true; 
    private Vector3 localScale;
    private Vector3 startPosition; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        startPosition = transform.position; 
    }

    private void FixedUpdate()
    {
        Move();
        if (verticalMovement) Flip();
    }

    private void Move()
    {
        // Cache values to reduce repeated property access
        var t = transform;
        Vector2 linearVelocity = rb.linearVelocity;

        if (verticalMovement)
            linearVelocity.y = (movingForward ? 1 : -1) * speed;
        else
            linearVelocity.x = (movingForward ? 1 : -1) * speed;

        rb.linearVelocity = linearVelocity;

        float position = verticalMovement ? t.position.y : t.position.x;
        float minLimit = verticalMovement ? startPosition.y + minBound : startPosition.x + minBound;
        float maxLimit = verticalMovement ? startPosition.y + maxBound : startPosition.x + maxBound;

        if ((movingForward && position >= maxLimit) || (!movingForward && position <= minLimit))
        {
            movingForward = !movingForward;
            if (!verticalMovement) Flip();
        }
    }

    private void Flip()
    {
        if (player != null)
        {
            if (verticalMovement)
            {
                localScale.x = transform.position.x > player.position.x ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            }
            else
            {
                localScale.x = movingForward ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
            }
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HandlePlayerCollision(collision);
        }
    }

    private void HandlePlayerCollision(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            if (collision.contacts[0].normal.y < -0.45f) // Player hit from above
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
                PlayerRespawn.RegisterDeactivatedEnemy(gameObject); // Update to use PlayerRespawn
                gameObject.SetActive(false);
            }
            else
            {
                PlayerRespawn playerRespawn = collision.collider.GetComponent<PlayerRespawn>();
                if (playerRespawn != null)
                {
                    playerRespawn.Respawn(); // Call the respawn method from PlayerRespawn
                }
            }
        }
    }
}
