using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public int damageToBase = 10;

    private PlayerBase playerBase;
    private bool isDead = false; // prevent double-kill

    private Rigidbody2D rb;

    void Start()
    {
        playerBase = FindAnyObjectByType<PlayerBase>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);

        // If enemy walks off screen left, count them as dead
        if (transform.position.x < -15f)
        {
            Die();
        }
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

        WaveManager wm = FindAnyObjectByType<WaveManager>();
        if (wm != null) wm.EnemyDied();

        if (ScoreManager.instance != null)
            ScoreManager.instance.AddScore(100);

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
            Die(); // use Die() everywhere — handles EnemyDied automatically
        }

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(5 * diff);
            Die();
        }
    }
}