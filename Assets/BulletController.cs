using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public int damage = 1;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Hit enemy soldier
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Hit enemy base
        EnemyBase enemyBase = other.GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}