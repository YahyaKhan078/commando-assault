using UnityEngine;
using TMPro;

public class AllyManager : MonoBehaviour
{
    public static AllyManager instance;

    [Header("Ally Settings")]
    public GameObject allyPrefab;
    public Transform playerTransform;   // spawn ally at player position
    public int allyCost = 500;

    [Header("UI")]
    public TextMeshProUGUI deployText;  // "DEPLOY [SPACE] - 500pts"

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateDeployUI();
    }

    void UpdateDeployUI()
    {
        if (deployText == null) return;

        int score = ScoreManager.instance != null
            ? ScoreManager.instance.GetScore() : 0;

        if (score >= allyCost)
        {
            // Make sure this says F not Space
            deployText.text = "[ F ] DEPLOY ALLY - 500pts";
            deployText.color = Color.green;
        }
        else
        {
            deployText.text = "[ F ] DEPLOY ALLY - 500pts";
            deployText.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }

    public void TryDeployAlly()
    {
        if (ScoreManager.instance == null) return;

        int score = ScoreManager.instance.GetScore();

        if (score < allyCost)
        {
            // Flash the text red briefly to show "can't afford"
            StartCoroutine(FlashCantAfford());
            return;
        }

        // Deduct score and spawn
        ScoreManager.instance.SpendScore(allyCost);

        Vector3 spawnPos = new Vector3(
            playerTransform.position.x + 1f, // slightly ahead of player
            playerTransform.position.y,
            0
        );

        Instantiate(allyPrefab, spawnPos, Quaternion.identity);
    }

    System.Collections.IEnumerator FlashCantAfford()
    {
        if (deployText == null) yield break;
        deployText.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        deployText.color = new Color(1f, 1f, 1f, 0.3f);
    }
}