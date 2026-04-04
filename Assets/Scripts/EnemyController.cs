using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public int damageToBase = 10;    // Increased for 100+ HP scale

    private PlayerBase playerBase;

    void Start()
    {
        playerBase = FindAnyObjectByType<PlayerBase>();
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        WaveManager wm = FindAnyObjectByType<WaveManager>();
        if (wm != null) wm.EnemyDied();

        // Standard kill reward
        if (ScoreManager.instance != null)
            ScoreManager.instance.AddScore(100);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int diff = MainMenuController.difficulty;

        if (other.CompareTag("PlayerBase"))
        {
            if (playerBase != null) playerBase.TakeDamage(damageToBase * diff);
            SignalManagerAndDestroy();
            return; // ADD THIS return so it doesn't check Player too
        }

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(5 * diff);
            SignalManagerAndDestroy();
        }
    }

    // Helper to ensure WaveManager always knows when an enemy is gone
    void SignalManagerAndDestroy()
    {
        WaveManager wm = FindAnyObjectByType<WaveManager>();
        if (wm != null) wm.EnemyDied();
        Destroy(gameObject);
    }
}