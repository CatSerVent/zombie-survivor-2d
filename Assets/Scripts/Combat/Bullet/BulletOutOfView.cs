using UnityEngine;

/// <summary>
/// 카메라 뷰포트 밖으로 나간 총알을 자동으로 풀로 반환.
/// </summary>
[DisallowMultipleComponent]
public sealed class BulletOutOfView : MonoBehaviour
{
    [Header("뷰포트 마진")]
    [Tooltip("뷰포트 밖으로 몇만큼 벗어나면 반환할지")]
    public float margin = 2f;

    private Camera cam;
    private PooledObject p;

    void Awake()
    {
        cam = Camera.main;
        p = GetComponent<PooledObject>();
    }

    void Update()
    {
        if (!cam) return;
        Vector3 v = cam.WorldToViewportPoint(transform.position);
        if (v.x < -margin || v.x > 1f + margin || v.y < -margin || v.y > 1f + margin)
        {
            if (p != null) p.Return();
        }
    }
}
