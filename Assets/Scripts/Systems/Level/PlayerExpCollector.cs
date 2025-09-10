using UnityEngine;

/// <summary>
/// 플레이어 주변의 ExpOrb를 자동으로 흡수하는 컴포넌트.
/// PassiveInventory의 Magnet 패시브로 수집 반경이 증가.
/// </summary>
[DisallowMultipleComponent]
public sealed class PlayerExpCollector : MonoBehaviour
{
    [Header("수집 반경 기본값")]
    [Tooltip("플레이어가 경험치를 자동으로 흡수하는 기본 반경")]
    public float baseRadius = 1f;

    [Header("레벨 시스템 참조")]
    [Tooltip("경험치를 더할 LevelSystem")]
    public LevelSystem levelSystem;

    [Header("수집 속도")]
    [Tooltip("자석처럼 끌어오는 속도")]
    public float magnetSpeed = 5f;

    private float radiusMul = 1f;

    void Update()
    {
        // Magnet 패시브를 통해 radiusMul 값을 외부에서 설정하도록 구현
        float finalRadius = baseRadius * radiusMul;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, finalRadius, LayerMask.GetMask("Exp"));

        foreach (var hit in hits)
        {
            var orb = hit.GetComponent<ExpOrb>();
            if (orb != null)
            {
                orb.MoveTowards(transform.position, magnetSpeed);
                if (Vector3.Distance(transform.position, orb.transform.position) < 0.2f)
                {
                    levelSystem.AddExp(orb.value);
                    orb.ReturnToPool();
                }
            }
        }
    }

    public void SetMagnetMultiplier(float mul) => radiusMul = mul;
}
