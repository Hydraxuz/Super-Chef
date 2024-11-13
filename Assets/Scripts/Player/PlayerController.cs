using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 12f;
    public float jumpSpeed = 18f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private Animator playerAnimation;

    private Vector2 moveInput; // Store movement input
    private PlayerInputActions inputActions; // Declare input actions variable

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();

        inputActions = new PlayerInputActions(); // Initialize the input actions
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

    private void Update()
    {
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        HandleMovement();
    }

    private void HandleMovement()
    {
        player.linearVelocity = new Vector2(moveInput.x * speed, player.linearVelocity.y);
        if (moveInput.x != 0f)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isTouchingGround)
        {
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpSpeed);
        }
    }

    private void UpdateAnimations()
    {
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.linearVelocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);
    }
}
