using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform groundCheckTransform; // The transform used to check for ground
    public float groundCheckRadius = 0.1f; // The radius for checking ground contact
    public LayerMask groundLayer; // Layer to identify what counts as ground

    public bool IsTouchingGround { get; private set; } // Indicates if the player is grounded

    public void UpdateGroundCheck()
    {
        // Reset the ground state values at the start of each check
        IsTouchingGround = false;
        // Ensure we have a valid transform to sample from
        Vector3 checkPos = groundCheckTransform != null ? groundCheckTransform.position : transform.position;

        // Perform an overlap circle check at the ground check position
        Collider2D groundHit = Physics2D.OverlapCircle(checkPos, groundCheckRadius, groundLayer);

        IsTouchingGround = groundHit != null;
    }
}