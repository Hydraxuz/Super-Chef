using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 12f; // Base movement speed
    public float jumpHeight = 18f; // Jump force
    private Vector2 moveInput; // Stores movement input

    public PhysicsMaterial2D slipMaterial; // Physics material for air
    public PhysicsMaterial2D stickMaterial; // Physics material for ground

    public float coyoteTime = 0.1f; // Coyote time duration
    public float coyoteCounter; // Coyote time counter

    private Rigidbody2D player; // Player's Rigidbody2D
    private Animator playerAnimation; // Player's Animator
    private GroundCheck groundCheck; // Reference to the GroundCheck component
    private PlayerInputActions inputActions; // Input actions for player controls

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        groundCheck = GetComponent<GroundCheck>(); // Get the GroundCheck component

        inputActions = new PlayerInputActions(); // Create a new instance of the input actions
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

        coyoteCounter = groundCheck.IsTouchingGround ? coyoteTime : coyoteCounter - Time.deltaTime;

        HandleMovement(); // Handle movement in physics updates
        UpdateAnimations(); // Update player animations
        UpdateMaterial(); // Update physics material based on ground state and input
    }

    private void HandleMovement()
    {
        // Normal movement
        player.linearVelocity = new Vector2(moveInput.x * speed, player.linearVelocity.y);

        if (moveInput.x != 0f)
        {
            // Flip player based on movement direction
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Read movement input

        // Switch to slip material when there is movement input
        if (moveInput.x != 0f)
        {
            player.sharedMaterial = slipMaterial; // Apply slip material when moving
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero; // Reset movement input
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (groundCheck.IsTouchingGround || coyoteCounter > 0f)
        {
            // Apply jump force if grounded
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpHeight);
            coyoteCounter = 0f; // Reset coyote time counter
        }
    }

    private void UpdateAnimations()
    {
        // Update animations based on movement and ground state
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.linearVelocity.x));
        playerAnimation.SetBool("OnGround", groundCheck.IsTouchingGround);
    }

    private void UpdateMaterial()
    {
        // If grounded and no input, switch to stick material
        if (groundCheck.IsTouchingGround && moveInput.x == 0f)
        {
            if (player.sharedMaterial != stickMaterial)
            {
                player.sharedMaterial = stickMaterial;
            }
        }
        // If not grounded, use slip material
        else if (!groundCheck.IsTouchingGround)
        {
            if (player.sharedMaterial != slipMaterial)
            {
                player.sharedMaterial = slipMaterial;
            }
        }
        // Otherwise, if there's input, use slip material
        else if (moveInput.x != 0f)
        {
            if (player.sharedMaterial != slipMaterial)
            {
                player.sharedMaterial = slipMaterial;
            }
        }
    }
}
