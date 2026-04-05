using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 originalPos;

    void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.2f, float magnitude = 0.15f)
    {
        StopAllCoroutines();
        StartCoroutine(DoShake(duration, magnitude));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(
                originalPos.x + x,
                originalPos.y + y,
                originalPos.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}