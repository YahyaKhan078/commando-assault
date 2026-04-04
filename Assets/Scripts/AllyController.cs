using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float speed = 2.5f;
    public int health = 3;
    private bool isDead = false;

    void Update()
    {
        if (isDead) return;
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > 20f)
            Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Destroy(gameObject);
    }

    // Detects non-trigger colliders (solid objects)
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EnemyController ec = col.gameObject.GetComponent<EnemyController>();
            if (ec != null) ec.TakeDamage(999);
            Die();
        }
    }

    // Detects trigger colliders (EnemyBase is a trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBase"))
        {
            EnemyBase eb = other.GetComponent<EnemyBase>();
            if (eb != null) eb.TakeDamage(5);
            Die();
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyController ec = other.GetComponent<EnemyController>();
            if (ec != null) ec.TakeDamage(999);
            Die();
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}