using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 15f;
    public int damage = 1;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotate bullet to face direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

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