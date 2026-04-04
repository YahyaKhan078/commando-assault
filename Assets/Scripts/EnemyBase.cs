using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;

    [Header("Protection Settings")]
    public bool isVulnerable = false;
    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    // THIS IS THE METHOD THE ERROR IS MISSING
    public void SetVulnerability(bool status)
    {
        isVulnerable = status;
        UpdateVisual();
    }

    public void TakeDamage(int damage)
    {
        if (!isVulnerable) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameManager.instance.WinGame();
            Destroy(gameObject);
        }
    }

    void UpdateVisual()
    {
        if (sr == null) return;

        // Grey = Protected (i.e., not vulnerable)
        // Orange = Attackable (i.e., vulnerable)
        sr.color = isVulnerable
            ? new Color(1f, 0.5f, 0f)    // Bright Orange
            : new Color(0.3f, 0.3f, 0.3f); // Dark Grey
    }
}