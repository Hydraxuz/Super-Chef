using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(GroundCheck))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 12f;
    public float jumpHeight = 18f;

    [Header("Physics")]
    public PhysicsMaterial2D slipMaterial;
    public PhysicsMaterial2D stickMaterial;
    public float coyoteTime = 0.1f;
    public float fallMultiplier = 2f;

    private float coyoteCounter;
    private Vector2 moveInput;

    private Rigidbody2D player;
    private Animator playerAnimation;
    private GroundCheck groundCheck;
    private PlayerInputActions inputActions;
    private float initialScaleX;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        groundCheck = GetComponent<GroundCheck>();

        inputActions = new PlayerInputActions();
        initialScaleX = transform.localScale.x;
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Disable();
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }

    private void FixedUpdate()
    {
        groundCheck.UpdateGroundCheck();

        if (groundCheck.IsTouchingGround)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.fixedDeltaTime;

        HandleMovement();
        ApplyGravityModifier();
        UpdateAnimations();
        UpdateMaterial();
    }

    private void HandleMovement()
    {
        player.linearVelocity = new Vector2(moveInput.x * speed, player.linearVelocity.y);

        if (moveInput.x != 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(initialScaleX) * Mathf.Sign(moveInput.x);
            transform.localScale = scale;
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
        if (context.performed)
            TryJump();
    }

    private void TryJump()
    {
        if (groundCheck.IsTouchingGround || coyoteCounter > 0f)
        {
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpHeight);
            coyoteCounter = 0f;
        }
    }

    private void UpdateAnimations()
    {
        playerAnimation.SetFloat("Speed", Mathf.Abs(player.linearVelocity.x));
        playerAnimation.SetBool("OnGround", groundCheck.IsTouchingGround);
    }

    private void UpdateMaterial()
    {
        PhysicsMaterial2D desiredMaterial =
            groundCheck.IsTouchingGround && moveInput.x == 0f
                ? stickMaterial
                : slipMaterial;

        if (player.sharedMaterial != desiredMaterial)
            player.sharedMaterial = desiredMaterial;
    }

    private void ApplyGravityModifier()
    {
        player.gravityScale = player.linearVelocity.y < 0f ? fallMultiplier : 1f;
    }
}