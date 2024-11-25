using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 12f; // Base movement speed
    public float jumpHeight = 18f; // Jump force
    private Rigidbody2D player; // Player's Rigidbody2D
    private Animator playerAnimation; // Player's Animator
    private Vector2 moveInput; // Stores movement input
    private PlayerInputActions inputActions; // Input actions for player controls

    private GroundCheck groundCheck; // Reference to the GroundCheck component

    public PhysicsMaterial2D normalMaterial; // Physics material for flat ground
    public PhysicsMaterial2D slopeMaterial; // Physics material for slopes

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        groundCheck = GetComponent<GroundCheck>(); // Get the GroundCheck component

        inputActions = new PlayerInputActions(); // Initialize input actions
    }

    private void OnEnable()
    {
        inputActions.Enable(); // Enable the input actions

        // Subscribe to input events
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Disable(); // Disable the input actions
    }

    private void FixedUpdate()
    {
        // Update ground check
        groundCheck.UpdateGroundCheck(); // Call the ground check update method
        
        HandleMovement(); // Handle movement in physics updates
        UpdateAnimations(); // Update player animations
    }

    private void HandleMovement()
    {
        if (groundCheck.IsOnSlope)
        {
            // Switch to slope material if on a slope with no input
            if (moveInput.x == 0f)
            {
                player.sharedMaterial = slopeMaterial; // Apply slope material
            }
            else
            {
                player.sharedMaterial = normalMaterial; // Apply normal material
            }

            // Adjust movement speed based on slope angle
            float slopeFactor = Mathf.Cos(groundCheck.SlopeAngle * Mathf.Deg2Rad);
            player.linearVelocity = new Vector2(moveInput.x * speed * slopeFactor, player.linearVelocity.y);
        }
        else
        {
            // Normal movement on flat ground
            player.sharedMaterial = normalMaterial; // Apply normal material
            player.linearVelocity = new Vector2(moveInput.x * speed, player.linearVelocity.y);
        }

        if (moveInput.x != 0f)
        {
            // Flip player based on movement direction
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Read movement input
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero; // Reset movement input
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (groundCheck.IsTouchingGround)
        {
            // Apply jump force if grounded
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpHeight);
        }
    }

    private void UpdateAnimations()
    {
        // Update animations based on movement and ground state
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.linearVelocity.x));
        playerAnimation.SetBool("OnGround", groundCheck.IsTouchingGround);
    }
}
