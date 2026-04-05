using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public int damageToBase = 10;

    public GameObject explosionPrefab;

    // ADD THESE TWO:
    public GameObject healthPickupPrefab;
    public GameObject ammoPickupPrefab;

    private PlayerBase playerBase;
    private bool isDead = false;
    private Rigidbody2D rb;

    void Start()
    {
        playerBase = FindAnyObjectByType<PlayerBase>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        if (transform.position.x < -15f) Die();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Explosion effect
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Track kills for pickup drops
        if (ScoreManager.instance != null)
            ScoreManager.instance.AddScore(100);

        WaveManager wm = FindAnyObjectByType<WaveManager>();
        if (wm != null) wm.EnemyDied();

        // DROP PICKUP — only every N kills based on difficulty
        // Easy=10, Medium=15, Hard=30
        int[] killsNeeded = { 10, 15, 30 };
        int diff = Mathf.Clamp(MainMenuController.difficulty - 1, 0, 2);
        int threshold = killsNeeded[diff];

        PickupDropManager.instance?.TryDropPickup(
            transform.position, threshold,
            healthPickupPrefab, ammoPickupPrefab
        );

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        int diff = MainMenuController.difficulty;

        if (other.CompareTag("PlayerBase"))
        {
            if (playerBase != null)
                playerBase.TakeDamage(damageToBase * diff);
            Die();
        }

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(5 * diff);
            Die();
        }
    }
}