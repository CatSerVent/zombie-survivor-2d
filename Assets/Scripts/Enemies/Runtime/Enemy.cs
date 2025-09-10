using Game.Combat.Bullet;
using UnityEngine;

namespace Game.Enemies.Runtime
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int hp = 10;

        public void Setup(int hp) => this.hp = hp;

        public void TakeDamage(int dmg, Vector3 hitPos)
        {
            hp -= dmg;

            // ✅ 무조건 이펙트 생성
            if (HitPoolManager.I != null)
            {
                var sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    HitPoolManager.I.Get(hitPos, sr.sprite);
                }
            }

            if (hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (ExpOrbPoolManager.I != null)
            {
                ExpOrbPoolManager.I.Spawn(1, transform.position); // 경험치 값 1 (혹은 expValue 변수)
            }

            Destroy(gameObject);
        }

    }
}
