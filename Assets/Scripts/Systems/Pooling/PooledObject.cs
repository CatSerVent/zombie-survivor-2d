using UnityEngine;

/// <summary>
/// Ǯ���� ������Ʈ�� �ڽ��� Ǯ�� ��ȯ�� �� �ְ� �ϴ� ������Ʈ.
/// </summary>
public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPool<Component> pool;
    private bool inPool;

    public bool InPool => inPool;

    void OnEnable() => inPool = false;

    public void Return()
    {
        if (inPool) return; // �ߺ� ��ȯ ����
        inPool = true;

        if (pool != null) pool.Return(this as Component);
        else PoolUtil.SafeRelease(gameObject);
    }
}
