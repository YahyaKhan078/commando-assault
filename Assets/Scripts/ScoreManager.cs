using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private int score = 0;
    private int highScore = 0;

    void Awake()
    {
        instance = this;
        // PlayerPrefs saves data between sessions
        // like a tiny save file on your computer
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Update()
    {
        scoreText.text = "Score: " + score;
        highScoreText.text = "Best: " + highScore;
    }

    public void AddScore(int points)
    {
        score += points;

        // New high score — save it immediately
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public int GetScore()
    {
        return score;
    }
    public void SpendScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
    }
}