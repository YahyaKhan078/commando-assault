using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Static instance — any script can call GameManager.instance from anywhere
    public static GameManager instance;

    [Header("UI")]
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI baseHPText;
    public TextMeshProUGUI enemyBaseHPText;
    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("References")]
    public PlayerBase playerBase;
    public EnemyBase enemyBase;
    public PlayerHealth playerHealth;

    void Awake()
    {
        // Singleton pattern — ensures only one GameManager exists
        instance = this;
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (playerHPText && playerHealth != null)
            playerHPText.text = "HP: " + playerHealth.currentHealth;

        if (baseHPText && playerBase != null)
            baseHPText.text = "Base: " + playerBase.currentHealth;

        if (enemyBaseHPText && enemyBase != null)
            enemyBaseHPText.text = "Enemy Base: " + enemyBase.currentHealth;
    }

    public TextMeshProUGUI finalScoreText;  // add this variable at top

    public void WinGame()
    {
        Time.timeScale = 0f;
        FindAnyObjectByType<WaveManager>()?.StopWaves();

        // Show final score on win screen
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + ScoreManager.instance.GetScore();

        if (winScreen != null) winScreen.SetActive(true);
    }

    public void LoseGame()
    {
        Time.timeScale = 0f;
        FindAnyObjectByType<WaveManager>()?.StopWaves();
        if (loseScreen != null) loseScreen.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // unpause before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}