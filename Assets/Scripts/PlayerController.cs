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

        // ADD THIS LINE — shows green if grounded, red if not
        Debug.DrawRay(groundCheck.position, Vector2.down * 0.1f,
            isGrounded ? Color.green : Color.red);
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
        if (GunManager.instance == null) return;
        if (!GunManager.instance.HasAmmo()) return;

        if (Mouse.current.leftButton.isPressed &&
            Time.time > nextFireTime)
        {
            nextFireTime = Time.time + GunManager.instance.GetFireRate();

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            // Flip player to face shoot direction
            if (direction.x < 0)
                transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            else
                transform.localScale = new Vector3(0.5f, 0.5f, 1f);

            // Spawn bullets (shotgun fires multiple)
            int bulletCount = GunManager.instance.GetBulletCount();
            float spread = GunManager.instance.GetSpread();

            for (int i = 0; i < bulletCount; i++)
            {
                // Calculate spread angle for each bullet
                float angle = 0f;
                if (bulletCount > 1)
                    angle = -spread + (spread * 2f / (bulletCount - 1)) * i;

                Vector2 spreadDir = RotateVector(direction, angle);
                Vector3 spawnPos = transform.position + (Vector3)(direction * 0.6f);

                GameObject bullet = Instantiate(bulletPrefab, spawnPos,
                                                Quaternion.identity);
                BulletController bc = bullet.GetComponent<BulletController>();
                bc.SetDirection(spreadDir);
                bc.damage = GunManager.instance.GetDamage();
            }

            GunManager.instance.UseAmmo();
        }
    }

    // Helper to rotate a vector by degrees
    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        );
    }
}