using UnityEngine;

/// <summary>
/// 적 유닛의 HP와 피해 처리, 사망 시 경험치 오브 생성.
/// </summary>
[DisallowMultipleComponent]
public sealed class Enemy : MonoBehaviour
{
    [Header("HP 설정")]
    [Tooltip("최대 HP")]
    public int maxHp = 10;

    private int hp;

    void OnEnable()
    {
        hp = maxHp;
        EnemyCounter.I?.Add(this);
    }

    public void Setup(int hpValue)
    {
        maxHp = hpValue;
        hp = maxHp;
    }

    public void TakeDamage(int dmg, Vector3 hitPos)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Die(hitPos);
        }
    }

    void Die(Vector3 pos)
    {
        EnemyCounter.I?.Remove(this);

        // 경험치 오브 생성
        var orb = ExpOrbPool.I?.Get(transform.position);
        if (orb != null)
        {
            orb.value = 1; // 경험치 값 지정
        }

        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        EnemyCounter.I?.Remove(this);
    }
}
