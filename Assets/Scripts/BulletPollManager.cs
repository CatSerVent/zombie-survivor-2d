using UnityEngine;
using Game.Systems;
using Game.Combat.Bullet;

[DisallowMultipleComponent]
public sealed class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager I;

    [Header("Prefab")]
    [SerializeField] private BulletHitPierce prefab;

    [Header("Pool Settings")]
    [SerializeField] private int initialCapacity = 50;
    [SerializeField] private int maxSize = 200;

    public ObjectPool<BulletHitPierce> Pool { get; private set; }

    private void Awake()
    {
        I = this;
        if (prefab == null)
        {
            Debug.LogError("[BulletPoolManager] Prefab이 비어있습니다. Inspector에서 BulletHitPierce 프리팹을 할당하세요.");
            return;
        }

        Pool = new ObjectPool<BulletHitPierce>(
            prefab,
            initialCapacity,
            maxSize,
            this.transform
        );
    }
}
