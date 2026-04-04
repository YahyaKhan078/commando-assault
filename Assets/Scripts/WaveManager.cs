using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    [Header("Wave Settings")]
    public int baseEnemyCount = 5;
    public float baseEnemySpeed = 2f;
    public float timeBetweenSpawns = 1.5f;
    public float timeBetweenWaves = 3f;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesLeftText;
    public TextMeshProUGUI waveAnnouncementText;

    private int currentWave = 0;
    private int enemiesAliveInScene = 0;
    private bool isSpawning = false;
    private bool gameActive = true;

    void Start()
    {
        if (waveAnnouncementText != null)
            waveAnnouncementText.gameObject.SetActive(false);

        StartNextWave();
    }

    void Update()
    {
        if (!gameActive) return;

        // UI Updates
        if (waveText != null)
            waveText.text = "WAVE: " + currentWave;

        if (enemiesLeftText != null)
        {
            if (isSpawning)
                enemiesLeftText.text = "INCOMING...";
            else
                enemiesLeftText.text = "CLEAR: " + enemiesAliveInScene + " REMAINING";

            // Visual feedback: red color when only 1 enemy left to hunt
            enemiesLeftText.color = (enemiesAliveInScene == 1) ? Color.red : Color.white;
        }

        // Logic to trigger next wave
        if (!isSpawning && enemiesAliveInScene <= 0 && currentWave > 0)
        {
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        currentWave++;

        int diff = MainMenuController.difficulty;

        // AGGRESSIVE MATH FOR HARD MODE
        // Hard (diff 3) = 12, 16, 20 enemies... starting speed 6.0!
        int enemiesToSpawn = (baseEnemyCount + (currentWave - 1) * 4) * diff;
        float enemySpeed = (baseEnemySpeed * diff) + (currentWave * 0.3f);

        // Spawn interval gets tighter as waves progress
        float currentSpawnDelay = (timeBetweenSpawns / diff) - (currentWave * 0.05f);
        currentSpawnDelay = Mathf.Max(currentSpawnDelay, 0.3f); // Don't go below 0.3s or it breaks the engine

        // Protect the base at wave start
        EnemyBase enemyBase = FindAnyObjectByType<EnemyBase>();
        if (enemyBase != null) enemyBase.SetVulnerability(false);

        StartCoroutine(SpawnWave(enemiesToSpawn, enemySpeed, currentSpawnDelay));
        StartCoroutine(AnnounceWave(currentWave));
    }

    System.Collections.IEnumerator SpawnWave(int count, float speed, float delay)
    {
        isSpawning = true;

        // This wait matches the countdown in AnnounceWave
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(speed);

            // Unlock base after 4th enemy (index 3)
            if (i == 3)
            {
                EnemyBase eb = FindAnyObjectByType<EnemyBase>();
                if (eb != null) eb.SetVulnerability(true);
            }

            yield return new WaitForSeconds(delay);
        }

        isSpawning = false;
    }

    void SpawnEnemy(float speed)
    {
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, -3.5f, 0);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        enemiesAliveInScene++;

        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null) ec.speed = speed;
    }

    public void EnemyDied()
    {
        enemiesAliveInScene--;
    }

    // --- THE NEW COUNTDOWN LOGIC ---
    System.Collections.IEnumerator AnnounceWave(int wave)
    {
        waveAnnouncementText.gameObject.SetActive(true);

        // We use the timeBetweenWaves variable to determine how long to count
        float counter = timeBetweenWaves;

        while (counter > 0)
        {
            waveAnnouncementText.text = "WAVE " + wave + "\nIN " + counter.ToString("F0") + "...";
            yield return new WaitForSeconds(1f);
            counter--;
        }

        waveAnnouncementText.text = "FIGHT!";
        yield return new WaitForSeconds(0.5f);
        waveAnnouncementText.gameObject.SetActive(false);
    }

    public void StopWaves()
    {
        gameActive = false;
        StopAllCoroutines();
    }
}