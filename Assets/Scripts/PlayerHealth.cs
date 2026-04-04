using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Updated to 100
    public int currentHealth;
    public TextMeshProUGUI hpText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHPUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Prevent health from going below 0
        if (currentHealth < 0) currentHealth = 0;

        UpdateHPUI();

        if (currentHealth <= 0)
        {
            GameManager.instance.LoseGame();
        }
    }

    // New method to heal the player (for those future loot drops!)
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHPUI();
    }

    void UpdateHPUI()
    {
        if (hpText != null)
        {
            hpText.text = "PLAYER HP: " + currentHealth;

            // Visual feedback: Text turns red when health is low
            if (currentHealth <= 25) hpText.color = Color.red;
            else hpText.color = Color.white;
        }
    }
}