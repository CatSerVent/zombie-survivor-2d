using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smooth = 5f;

    void LateUpdate()
    {
        if (!target) return;

        // �÷��̾� ��ġ + ���������� �ε巴�� �̵�
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smooth * Time.deltaTime
        );
    }
}
