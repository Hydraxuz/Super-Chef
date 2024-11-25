using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float rayLength = 0.1f; // Length of the ray for ground detection
    public LayerMask groundLayer; // Layer to identify what counts as ground

    private Collider2D playerCollider; // Reference to the player's collider

    public bool IsGrounded { get; private set; } // Public getter for the ground state

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>(); // Get the player's collider
    }

    private void FixedUpdate()
    {
        IsGrounded = false;

        // Cast a ray from the bottom of the collider
        Vector2 rayOrigin = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundLayer);

        if (hit.collider != null)
        {
            IsGrounded = true; // Set to true if the ray hits a ground layer object
        }
    }

    private void OnDrawGizmos()
    {
        // Optional: Visualize the raycast in the scene view
        if (playerCollider != null)
        {
            Vector2 rayOrigin = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y);
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * rayLength);
        }
    }
}
