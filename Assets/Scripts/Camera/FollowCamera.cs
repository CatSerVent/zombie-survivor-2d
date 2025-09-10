using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smooth = 5f;

    void LateUpdate()
    {
        if (!target) return;

        // 플레이어 위치 + 오프셋으로 부드럽게 이동
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smooth * Time.deltaTime
        );
    }
}
