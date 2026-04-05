using UnityEngine;

public class FloatingTextMover : MonoBehaviour
{
    public float speed = 1.5f;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}