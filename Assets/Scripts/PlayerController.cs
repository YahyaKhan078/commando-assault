using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 12f;

    [Header("Jump Feel")]
    public float fallMultiplier = 3f;      // how fast player falls down
    public float lowJumpMultiplier = 6f;   // fast fall if tap vs hold Space

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;
    private float nextFireTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
        HandleBetterGravity();
        HandleShooting();
        HandleAllyDeploy(); // ADD THIS LINE
    }

    void HandleAllyDeploy()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (AllyManager.instance != null)
                AllyManager.instance.TryDeployAlly();
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void HandleMovement()
    {
        Vector2 movement = Vector2.zero;

        if (Keyboard.current.dKey.isPressed) movement.x += 1;
        if (Keyboard.current.aKey.isPressed) movement.x -= 1;

        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);

        float clampedX = Mathf.Clamp(transform.position.x, -8f, 8f);
        transform.position = new Vector3(clampedX, transform.position.y, 0);
    }

    void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void HandleBetterGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Falling � apply extra gravity so player drops faster
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                         * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            // Rising but Space released � cut jump short for tap jump
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                         * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void HandleShooting()
    {
        if (Mouse.current.leftButton.isPressed && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );
            Vector2 direction = (mousePos - (Vector2)transform.position);

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position + Vector3.right * 0.5f,
                Quaternion.identity
            );
            bullet.GetComponent<BulletController>().SetDirection(direction);
        }
    }
}