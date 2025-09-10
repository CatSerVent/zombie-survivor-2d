using UnityEngine;

/// <summary>
/// 총알이 일정 시간 후 자동으로 풀로 반환되도록 하는 컴포넌트.
/// </summary>
[DisallowMultipleComponent]
public sealed class BulletLifetime : MonoBehaviour
{
    [Header("수명 설정")]
    [Tooltip("총알이 유지되는 시간(초)")]
    public float life = 2f;

    private float t;
    private PooledObject p;

    void OnEnable()
    {
        t = 0f;
        if (!p) p = GetComponent<PooledObject>();
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
