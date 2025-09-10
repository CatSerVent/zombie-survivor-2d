using UnityEngine;
using Game.Systems;

namespace Game.Combat.Bullet
{
    [DisallowMultipleComponent]
    public sealed class HitPoolManager : MonoBehaviour
    {
        public static HitPoolManager I;

        [Header("Prefab")]
        [SerializeField] private BulletHit hitPrefab;   // ✅ 다시 BulletHit

        [Header("Pool Settings")]
        [SerializeField] private int initialCapacity = 30;
        [SerializeField] private int maxSize = 100;

        private ObjectPool<BulletHit> hitPool;

        private void Awake()
        {
            I = this;
            hitPool = new ObjectPool<BulletHit>(hitPrefab, initialCapacity, maxSize, this.transform);
        }

        public BulletHit Get(Vector3 pos, Sprite enemySprite)
        {
            var inst = hitPool.Get();
            if (inst == null) return null;

            inst.Setup(enemySprite, pos);
            return inst;
        }

        public void Release(BulletHit hit)
        {
            if (hit == null) return;
            hitPool.Release(hit);
        }
    }
}
