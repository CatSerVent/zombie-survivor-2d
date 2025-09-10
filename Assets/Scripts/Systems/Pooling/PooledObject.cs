using UnityEngine;

/// <summary>
/// 풀링된 오브젝트가 자신을 풀로 반환할 수 있게 하는 컴포넌트.
/// </summary>
public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPool<Component> pool;
    private bool inPool;

    public bool InPool => inPool;

    void OnEnable() => inPool = false;

    public void Return()
    {
        if (inPool) return; // 중복 반환 방지
        inPool = true;

        if (pool != null) pool.Return(this as Component);
        else PoolUtil.SafeRelease(gameObject);
    }
}
