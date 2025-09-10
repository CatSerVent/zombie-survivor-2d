using UnityEngine;
using Game.Systems;

[DisallowMultipleComponent]
public sealed class ExpOrbPoolManager : MonoBehaviour
{
    public static ExpOrbPoolManager I;

    [Header("Prefab")]
    [SerializeField] private ExpOrb orbPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int initialCapacity = 50;
    [SerializeField] private int maxSize = 200;

    private ObjectPool<ExpOrb> pool;

    void Awake()
    {
        I = this;
        pool = new ObjectPool<ExpOrb>(orbPrefab, initialCapacity, maxSize, this.transform);
    }

    public void Spawn(int value, Vector3 pos)
    {
        var orb = pool.Get();
        if (orb == null) return;
        orb.SetValue(value, pos);
    }

    public void Release(ExpOrb orb)
    {
        if (orb == null) return;
        pool.Release(orb);
    }
}
