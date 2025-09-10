using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class ObjectPool<T> : IPool where T : Component
    {
        private readonly T prefab;
        private readonly Transform parent;
        private readonly Stack<T> pool = new Stack<T>();
        private int countAll;
        private readonly int maxSize;

        public ObjectPool(T prefab, int initialCapacity, int maxSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.maxSize = Mathf.Max(1, maxSize);
            this.parent = parent;

            for (int i = 0; i < initialCapacity; i++)
            {
                var inst = CreateInstance();
                Release(inst);
            }
        }

        private T CreateInstance()
        {
            var inst = Object.Instantiate(prefab, parent);
            inst.gameObject.SetActive(false);

            var p = inst.GetComponent<PooledObject>();
            if (p == null) p = inst.gameObject.AddComponent<PooledObject>();
            p.pool = this;   // ✅ 인터페이스로 연결

            countAll++;
            return inst;
        }

        public T Get()
        {
            if (pool.Count > 0)
            {
                return pool.Pop();
            }
            else if (countAll < maxSize)
            {
                return CreateInstance();
            }
            else
            {
                Debug.LogWarning($"[ObjectPool] MaxSize {maxSize} reached for {prefab.name}");
                return null;
            }
        }

        public void Release(T element)
        {
            if (element == null) return;
            element.gameObject.SetActive(false);
            pool.Push(element);
        }

        // 인터페이스 구현
        void IPool.Release(PooledObject obj)
        {
            if (obj == null) return;
            var comp = obj.GetComponent<T>();
            if (comp != null) Release(comp);
        }

        public int CountInactive => pool.Count;
        public int CountCreated => countAll;
        public int MaxSize => maxSize;
    }
}
