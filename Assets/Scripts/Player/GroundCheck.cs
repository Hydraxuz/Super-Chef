using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform groundCheckTransform; // The transform used to check for ground
    public float groundCheckRadius = 0.1f; // The radius for checking ground contact
    public LayerMask groundLayer; // Layer to identify what counts as ground

    public bool IsTouchingGround { get; private set; } // Indicates if the player is grounded
    public bool IsOnSlope { get; private set; } // Indicates if the player is on a slope
    public float SlopeAngle { get; private set; } // Angle of the slope the player is on

    private Collider2D playerCollider; // Reference to the player's collider
    private Vector2 slopeNormal; // Normal of the slope

    private void Awake()
    {
        if (groundCheckTransform == null)
        {
            Debug.LogError("GroundCheck: groundCheckTransform is not assigned!");
            enabled = false; // Disable script if no ground check transform is assigned
        }
    }

    public void UpdateGroundCheck()
    {
        // Reset the ground state values at the start of each check
        IsTouchingGround = false;
        IsOnSlope = false;
        SlopeAngle = 0f;

        // Perform an overlap circle check at the ground check position
        Collider2D groundHit = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);

        if (groundHit != null)
        {
            IsTouchingGround = true;

            // Get the slope normal by checking the contact point's normal
            Vector2 hitPoint = groundHit.ClosestPoint(groundCheckTransform.position);
            slopeNormal = (hitPoint - (Vector2)groundCheckTransform.position).normalized;

            // Calculate the slope angle
            SlopeAngle = Vector2.Angle(Vector2.up, slopeNormal);

            // Determine if the player is on a slope
            if (SlopeAngle > 0f && SlopeAngle <= 45f) // Adjust max slope angle as needed
            {
                IsOnSlope = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Optional: Visualize the ground check position in the scene view
        if (groundCheckTransform != null)
        {
            Gizmos.color = IsTouchingGround ? (IsOnSlope ? Color.yellow : Color.green) : Color.red;
            Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        }
    }
}
