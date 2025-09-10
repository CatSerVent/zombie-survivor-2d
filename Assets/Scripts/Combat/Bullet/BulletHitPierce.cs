using Game.Enemies.Runtime;
using Game.Systems;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class BulletHitPierce : MonoBehaviour
{
    public int damage = 1;
    public int pierce = 1;
    public float splashRadius = 0f;
    public float splashRatio = 0f;

    [SerializeField] private float lifeTime = 5f;

    private PooledObject p;
    private Collider2D col;
    private bool returned;
    private float spawnTime;

    private readonly HashSet<int> hitIds = new HashSet<int>();
    private static readonly Collider2D[] tmp = new Collider2D[16];
    private static int enemyMask = -1;

    void Awake()
    {
        p = GetComponent<PooledObject>();
        col = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        // ✅ 매번 초기화
        returned = false;
        hitIds.Clear();
        if (col) col.enabled = true;
        spawnTime = Time.time;

        if (enemyMask == -1)
            enemyMask = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if (!returned && Time.time - spawnTime > lifeTime)
        {
            ReturnToPool();
        }
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
                if (ex != null && ex != e)
                    ex.TakeDamage(Mathf.CeilToInt(damage * splashRatio), transform.position);
            }
        }

        pierce--;
        if (pierce <= 0)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (returned) return;
        returned = true;

        if (col) col.enabled = false;

        if (p != null) p.Return();
        else gameObject.SetActive(false);
    }
}
