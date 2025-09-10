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
            Debug.LogError("[BulletPoolManager] Prefab�� ����ֽ��ϴ�. Inspector���� BulletHitPierce �������� �Ҵ��ϼ���.");
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
