using UnityEngine;

/// <summary>
/// �÷��̾ ���� �̵��ϴ� ������ AI.
/// </summary>
[DisallowMultipleComponent]
public sealed class EnemyAI : MonoBehaviour
{
    [Header("�ӵ�")]
    [Tooltip("�⺻ �̵� �ӵ�")]
    public float speed = 2f;

    private Transform player;

    public void SetupSpeed(float s) => speed = s;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (!player) return;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
