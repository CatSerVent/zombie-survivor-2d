using UnityEngine;
using Game.Systems; // ✅ PooledObject 인식

namespace Game.Systems
{
    /// <summary>
    /// 풀링 유틸리티
    /// - 풀링된 오브젝트는 안전하게 Return()
    /// - 풀링이 아닌 경우 Destroy() 처리
    /// </summary>
    public static class PoolUtil
    {
        public static void SafeRelease(GameObject go)
        {
            if (go == null) return;

            var p = go.GetComponent<PooledObject>();
            if (p != null)
            {
                p.Return();
            }
            else
            {
                Object.Destroy(go);
            }
        }
    }
}
