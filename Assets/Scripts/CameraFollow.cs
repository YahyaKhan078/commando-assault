using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // drag your Player here
    public float smoothSpeed = 5f;    // how smoothly camera catches up
    public float offsetX = 2f;        // camera leads slightly ahead of player

    // Left and right limits so camera never shows beyond the battlefield
    public float minX = -8f;
    public float maxX = 8f;

    void LateUpdate()
    {
        // LateUpdate runs AFTER all movement — so camera never jitters

        // Target position: follow player X, stay fixed on Y
        float targetX = player.position.x + offsetX;

        // Clamp so camera doesn't scroll past battlefield edges
        targetX = Mathf.Clamp(targetX, minX, maxX);

        // Smoothly move toward target (not instant = feels natural)
        Vector3 targetPos = new Vector3(
            targetX,
            transform.position.y,  // Y never changes — horizontal only
            transform.position.z   // Z never changes — keep at -10
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }
}