using UnityEngine;

public class PickupController : MonoBehaviour
{
    public enum PickupType { Health, Ammo }
    public PickupType pickupType;

    public int healAmount = 20;
    public int ammoAmount = 30;

    private bool collected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        if (pickupType == PickupType.Health)
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.Heal(healAmount);
            ShowFloatingText("+20 HP", Color.green);
        }
        else
        {
            if (GunManager.instance != null)
                GunManager.instance.AddAmmo(ammoAmount);
            ShowFloatingText("+30 AMMO", Color.yellow);
        }

        Destroy(gameObject);
    }

    void ShowFloatingText(string msg, Color color)
    {
        // Spawn a floating text at pickup position
        GameObject textObj = new GameObject("FloatingText");
        textObj.transform.position = transform.position + Vector3.up * 0.5f;

        // Add TextMesh for world space text
        TextMesh tm = textObj.AddComponent<TextMesh>();
        tm.text = msg;
        tm.color = color;
        tm.fontSize = 24;
        tm.alignment = TextAlignment.Center;
        tm.anchor = TextAnchor.MiddleCenter;

        // Auto destroy after 1 second
        Destroy(textObj, 1f);

        // Float upward
        textObj.AddComponent<FloatingTextMover>();
    }

    // Auto destroy if not collected after 8 seconds
    void Start()
    {
        Destroy(gameObject, 8f);
    }
}