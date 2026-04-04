using UnityEngine;
using TMPro;

public class PlayerBase : MonoBehaviour
{
    public int maxHealth = 500;
    public int currentHealth;
    public TextMeshProUGUI baseHpText; // Drag your UI text here

    void Start()
    {
        currentHealth = maxHealth;
        UpdateBaseUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateBaseUI();

        if (currentHealth <= 0)
        {
            // If base falls, you lose the mission
            GameManager.instance.LoseGame();
        }
    }

    void UpdateBaseUI()
    {
        if (baseHpText != null)
        {
            baseHpText.text = "BASE HP: " + currentHealth;
            // Turn red if critical
            baseHpText.color = (currentHealth < 100) ? Color.red : Color.white;
        }
    }
}