using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 제네릭 오브젝트 풀 시스템.
/// Instantiate/Destroy를 피하고 재사용을 통해 성능을 향상.
/// </summary>
public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Stack<T> stack = new Stack<T>();

    public int CountInactive => stack.Count;

    public ObjectPool(T prefab, int initialCapacity, int maxSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialCapacity; i++)
        {
            var inst = CreateInstance();
            Return(inst);
        }
    }

    private T CreateInstance()
    {
        var inst = Object.Instantiate(prefab, parent);
        var p = inst.GetComponent<PooledObject>();
        if (p == null) p = inst.gameObject.AddComponent<PooledObject>();
        p.pool = this as ObjectPool<Component>;
        inst.gameObject.SetActive(false);
        return inst;
    }

    public T Get()
    {
        if (stack.Count == 0)
        {
            var inst = CreateInstance();
            return inst;
        }

        var obj = stack.Pop();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        stack.Push(obj);
    }

    public void Return(T obj) => Release(obj);
}
