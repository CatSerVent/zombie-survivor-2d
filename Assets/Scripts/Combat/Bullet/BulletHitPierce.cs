using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 관통/스플래시를 지원하는 총알 히트 처리.
/// Enemy에 데미지를 주고, pierce 횟수 소모 후 풀로 반환.
/// </summary>
[DisallowMultipleComponent]
public sealed class BulletHitPierce : MonoBehaviour
{
    [Header("피해 및 관통 설정")]
    [Tooltip("이 총알이 주는 피해")]
    public int damage = 1;

    [Tooltip("관통 가능한 횟수")]
    public int pierce = 1;

    [Header("스플래시 설정")]
    [Tooltip("스플래시 반경 (0이면 없음)")]
    public float splashRadius = 0f;

    [Tooltip("스플래시 비율 (0이면 없음)")]
    public float splashRatio = 0f;

    private PooledObject p;
    private Collider2D col;
    private bool returned;
    private readonly HashSet<int> hitIds = new HashSet<int>();

    static readonly Collider2D[] tmp = new Collider2D[16];
    static int enemyMask = -1;

    void Awake()
    {
        p = GetComponent<PooledObject>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (enemyMask == -1) enemyMask = LayerMask.GetMask("Enemy");
    }

    void OnEnable()
    {
        returned = false;
        hitIds.Clear();
        if (col) col.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (returned) return;
        var e = other.GetComponent<Enemy>();
        if (e == null) return;

        int id = e.GetInstanceID();
        if (hitIds.Contains(id)) return;
        hitIds.Add(id);

        e.TakeDamage(damage, transform.position);

        if (splashRadius > 0f && splashRatio > 0f)
        {
            int n = Physics2D.OverlapCircleNonAlloc(e.transform.position, splashRadius, tmp, enemyMask);
            for (int i = 0; i < n; i++)
            {
                var ex = tmp[i].GetComponent<Enemy>();
                if (ex != null && ex != e) ex.TakeDamage(Mathf.CeilToInt(damage * splashRatio), transform.position);
            }
        }

        pierce--;
        if (pierce <= 0)
        {
            returned = true;
            if (col) col.enabled = false;
            StartCoroutine(ReturnNextFrame());
        }
    }

    IEnumerator ReturnNextFrame()
    {
        yield return null;
        if (p != null) p.Return();
        else gameObject.SetActive(false);
    }
}
