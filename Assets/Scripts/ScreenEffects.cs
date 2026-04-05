using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenEffects : MonoBehaviour
{
    public static ScreenEffects instance;

    [Header("Damage Overlay")]
    public Image damageOverlay;       // drag DamageOverlay image here
    public float flashDuration = 0.3f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Persistent red pulse when HP is low
        PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
        if (ph != null && ph.currentHealth <= 25)
        {
            float pulse = (Mathf.Sin(Time.time * 4f) + 1f) / 2f;  // 0 to 1 pulsing
            damageOverlay.color = new Color(1f, 0f, 0f, pulse * 0.3f);
        }
        else if (damageOverlay.color.a > 0 && !isFlashing)
        {
            // Fade out when HP is back above 25
            Color c = damageOverlay.color;
            c.a = Mathf.MoveTowards(c.a, 0f, Time.deltaTime * 2f);
            damageOverlay.color = c;
        }
    }

    private bool isFlashing = false;

    public void FlashRed()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        isFlashing = true;
        damageOverlay.color = new Color(1f, 0f, 0f, 0.4f);
        yield return new WaitForSeconds(flashDuration);
        isFlashing = false;
    }
}