using UnityEngine;
using UnityEngine.UI;
using Game.Systems;   // ✅ PooledObject 인식

public class DamageText : MonoBehaviour
{
    public float life = 0.6f;
    public float rise = 1.2f;
    public Text txt;

    private float t;
    private Vector3 startWorld;
    private PooledObject p;
    private Transform cam;

    public void Set(int dmg, Vector3 worldPos)
    {
        if (txt) txt.text = dmg.ToString();
        startWorld = worldPos;
        t = 0f;
        if (p == null) p = GetComponent<PooledObject>();
        if (cam == null) cam = Camera.main ? Camera.main.transform : null;

        // ✅ WorldCanvasManager 싱글톤이 존재할 때만 부모 설정
        if (WorldCanvasManager.I != null)
            transform.SetParent(WorldCanvasManager.I.transform, false);

        transform.position = worldPos;

        if (txt != null)
        {
            var c = txt.color;
            c.a = 1f;
            txt.color = c;
        }

        transform.localScale = Vector3.one;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        t += dt;
        float a = t / life;

        // 위로 올라가는 애니메이션
        Vector3 pos = startWorld + Vector3.up * rise * a;
        transform.position = pos;

        // 알파 값 줄이기
        if (txt != null)
        {
            var c = txt.color;
            c.a = 1f - a;
            txt.color = c;
        }

        if (t >= life)
        {
            if (p != null) p.Return();
            else gameObject.SetActive(false);
        }
    }
}
