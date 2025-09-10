using UnityEngine;

namespace Game.Systems
{
    public class PooledObject : MonoBehaviour
    {
        [HideInInspector] public IPool pool;
        private bool inPool;

        public bool InPool => inPool;

        void OnEnable() => inPool = false;

        public void Return()
        {
            if (inPool) return;
            inPool = true;

            if (pool != null) pool.Release(this);
            else gameObject.SetActive(false);
        }
    }

    // Ǯ �������̽�
    public interface IPool
    {
        void Release(PooledObject obj);
    }
}
