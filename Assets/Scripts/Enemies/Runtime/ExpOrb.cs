using UnityEngine;
using System.Collections;
using Game.Systems;

[DisallowMultipleComponent]
public sealed class ExpOrb : MonoBehaviour
{
    private int value;
    private PooledObject pooled;
    private Collider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        pooled = GetComponent<PooledObject>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetValue(int v, Vector3 pos)
    {
        value = v;
        transform.position = pos;

        if (col != null) col.enabled = true;
        if (sr != null) sr.enabled = true;

        gameObject.SetActive(true);
    }

    /// <summary>플레이어 방향으로 흡수</summary>
    public void Absorb(Transform player, System.Action<int> onCollected)
    {
        StartCoroutine(MoveToPlayer(player, onCollected));
    }

    private IEnumerator MoveToPlayer(Transform player, System.Action<int> onCollected)
    {
        float speed = 12f; // 빨려 들어가는 속도
        while (player != null && Vector3.Distance(transform.position, player.position) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            speed += 30f * Time.deltaTime; // 가까워질수록 가속
            yield return null;
        }

        // 수집 완료
        if (col != null) col.enabled = false;
        if (sr != null) sr.enabled = false;

        onCollected?.Invoke(value);

        if (pooled != null) pooled.Return();
        else gameObject.SetActive(false);
    }
}
