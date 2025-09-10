using UnityEngine;
using System.Collections;
using Game.Systems;

public class BulletHit : MonoBehaviour
{
    private SpriteRenderer sr;
    private PooledObject pooled;

    public void Setup(Sprite enemySprite, Vector3 pos)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (pooled == null) pooled = GetComponent<PooledObject>();

        sr.sprite = enemySprite;
        sr.color = Color.white;
        transform.position = pos;

        gameObject.SetActive(true);

        StartCoroutine(AutoFade());
    }

    IEnumerator AutoFade()
    {
        float t = 0f;
        Color c = sr.color;
        while (t < 0.2f) // 0.2초만 유지
        {
            t += Time.deltaTime;
            sr.color = new Color(c.r, c.g, c.b, 1f - (t / 0.2f));
            yield return null;
        }

        if (pooled != null) pooled.Return();
        else gameObject.SetActive(false);
    }
}
