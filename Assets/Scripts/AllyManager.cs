using UnityEngine;
using TMPro;
using System.Collections;

public class AllyManager : MonoBehaviour
{
    public static AllyManager instance;

    [Header("Ally Settings")]
    public GameObject allyPrefab;
    public Transform playerTransform;
    public int allyCost = 500;

    [Header("UI")]
    public TextMeshProUGUI deployText;      // small persistent top text
    public TextMeshProUGUI deployAlertText; // big flash alert in center

    private bool alertShownThisThreshold = false;

    void Awake() { instance = this; }

    void Update()
    {
        UpdateDeployUI();
        CheckDeployAlert();
    }

    void UpdateDeployUI()
    {
        if (deployText == null) return;

        int score = ScoreManager.instance != null
            ? ScoreManager.instance.GetScore() : 0;

        if (score >= allyCost)
        {
            deployText.text = "[ F ] DEPLOY ALLY";
            deployText.color = Color.green;
        }
        else
        {
            deployText.text = "[ F ] DEPLOY ALLY - " + allyCost + "pts";
            deployText.color = new Color(1f, 1f, 1f, 0.4f);
        }
    }

    void CheckDeployAlert()
    {
        if (ScoreManager.instance == null) return;
        int score = ScoreManager.instance.GetScore();

        // Show alert exactly once when score crosses 500
        if (score >= allyCost && !alertShownThisThreshold)
        {
            alertShownThisThreshold = true;
            StartCoroutine(ShowDeployAlert());
        }

        // Reset so it can flash again next time score 
        // drops below (e.g. after spending)
        if (score < allyCost)
            alertShownThisThreshold = false;
    }

    IEnumerator ShowDeployAlert()
    {
        if (deployAlertText == null) yield break;

        deployAlertText.gameObject.SetActive(true);
        deployAlertText.text = "[ F ] DEPLOY ALLY!";

        // Flash green 3 times over 2.5 seconds
        for (int i = 0; i < 3; i++)
        {
            deployAlertText.color = Color.green;
            yield return new WaitForSeconds(0.4f);
            deployAlertText.color = new Color(0f, 1f, 0f, 0.2f);
            yield return new WaitForSeconds(0.4f);
        }

        deployAlertText.gameObject.SetActive(false);
    }

    public void TryDeployAlly()
    {
        if (ScoreManager.instance == null) return;

        int score = ScoreManager.instance.GetScore();
        if (score < allyCost)
        {
            StartCoroutine(FlashCantAfford());
            return;
        }

        ScoreManager.instance.SpendScore(allyCost);
        alertShownThisThreshold = false; // reset for next time

        Vector3 spawnPos = new Vector3(
            playerTransform.position.x + 1f,
            playerTransform.position.y,
            0
        );
        Instantiate(allyPrefab, spawnPos, Quaternion.identity);
    }

    IEnumerator FlashCantAfford()
    {
        if (deployText == null) yield break;
        deployText.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        deployText.color = new Color(1f, 1f, 1f, 0.4f);
    }
}