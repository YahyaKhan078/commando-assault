using UnityEngine;

public class PickupDropManager : MonoBehaviour
{
    public static PickupDropManager instance;

    private int killsSinceLastDrop = 0;

    void Awake() { instance = this; }

    public void TryDropPickup(Vector3 position, int threshold,
                               GameObject healthPrefab,
                               GameObject ammoPrefab)
    {
        killsSinceLastDrop++;

        if (killsSinceLastDrop < threshold) return;

        // Reset counter
        killsSinceLastDrop = 0;

        // Spawn above ground level
        Vector3 spawnPos = new Vector3(position.x, -2.5f, 0);

        // 50/50 health or ammo
        float roll = Random.Range(0f, 1f);
        if (roll < 0.5f && healthPrefab != null)
            Instantiate(healthPrefab, spawnPos, Quaternion.identity);
        else if (ammoPrefab != null)
            Instantiate(ammoPrefab, spawnPos, Quaternion.identity);
    }
}