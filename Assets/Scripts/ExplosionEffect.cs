using UnityEngine;
using System.Collections;

public class ExplosionEffect : MonoBehaviour
{
    public Sprite[] explosionFrames;  // drag sprites here
    public float frameDuration = 0.05f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(PlayExplosion());
    }

    IEnumerator PlayExplosion()
    {
        foreach (Sprite frame in explosionFrames)
        {
            sr.sprite = frame;
            yield return new WaitForSeconds(frameDuration);
        }
        Destroy(gameObject);
    }
}