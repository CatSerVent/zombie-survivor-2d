using UnityEngine;

/// <summary>
/// 오브젝트를 안전하게 반환하거나 Destroy하는 유틸리티.
/// </summary>
public static class PoolUtil
{
    public static void SafeRelease(GameObject go)
    {
        if (!go) return;
        var p = go.GetComponent<PooledObject>();
        if (p != null) p.Return();
        else Object.Destroy(go);
    }
}
