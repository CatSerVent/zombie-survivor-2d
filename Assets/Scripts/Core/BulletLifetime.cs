using UnityEngine;
using Game.Systems; // ✅ PooledObject 정의된 네임스페이스

[DisallowMultipleComponent]
public sealed class BulletLifetime : MonoBehaviour
{
    [Tooltip("총알의 생존 시간 (초)")]
    public float life = 2f;

    private float t;
    private PooledObject p;

    void Awake()
    {
        p = GetComponent<PooledObject>();
    }

    void OnEnable()
    {
        t = 0f;
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t >= life)
        {
            if (p != null) p.Return();
            else gameObject.SetActive(false);
        }
    }
}
