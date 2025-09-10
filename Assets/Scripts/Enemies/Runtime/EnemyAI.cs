using UnityEngine;

/// <summary>
/// 플레이어를 향해 이동하는 간단한 AI.
/// </summary>
[DisallowMultipleComponent]
public sealed class EnemyAI : MonoBehaviour
{
    [Header("속도")]
    [Tooltip("기본 이동 속도")]
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
