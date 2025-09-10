using UnityEngine;
using Game.Systems; // ✅ PooledObject 정의된 네임스페이스 추가

[DisallowMultipleComponent]
public sealed class BulletOutOfView : MonoBehaviour
{
    [Tooltip("화면 밖으로 나갔을 때 추가 여유값 (뷰포트 좌표 기준)")]
    public float margin = 0.1f; // ✅ 원래 2f는 뷰포트 좌표에서 너무 큼 (1이 화면 끝)

    private Camera cam;
    private PooledObject p;

    void Awake()
    {
        p = GetComponent<PooledObject>();
    }

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (!cam) return;

        Vector3 v = cam.WorldToViewportPoint(transform.position);

        // ✅ viewport 좌표: 0~1이 화면 안, <0 또는 >1이면 화면 밖
        if (v.x < -margin || v.x > 1f + margin || v.y < -margin || v.y > 1f + margin)
        {
            if (p != null) p.Return();
            else gameObject.SetActive(false);
        }
    }
}
