using UnityEngine;

/// <summary>
/// BulletHit 이펙트 전용 풀 매니저.
/// BulletHit 이펙트 프리팹을 풀에서 꺼내 사용.
/// </summary>
[DisallowMultipleComponent]
public sealed class HitPoolManager : MonoBehaviour
{
    public static HitPoolManager I;

    [Header("Prefab")]
    [Tooltip("히트 이펙트 프리팹 (PooledObject 포함)")]
    [SerializeField] private BulletHit hitPrefab;

    [Header("Pool Settings")]
    [Tooltip("시작 시 미리 생성할 개수")]
    [SerializeField] private int initialCapacity = 30;

    [Tooltip("풀 최대 크기")]
    [SerializeField] private int maxSize = 100;

    private ObjectPool<BulletHit> hitPool;

    void Awake()
    {
        I = this;
        hitPool = new ObjectPool<BulletHit>(hitPrefab, initialCapacity, maxSize, transform);
    }

    public BulletHit Get(Vector3 position, Quaternion rotation)
    {
        var inst = hitPool.Get();
        if (inst == null) return null;
        inst.transform.SetParent(null);
        inst.transform.SetPositionAndRotation(position, rotation);
        inst.gameObject.SetActive(true);
        return inst;
    }

    public void Release(BulletHit hit)
    {
        if (hit == null) return;
        hitPool.Release(hit);
    }

    public int CountInactive => hitPool.CountInactive;
}
