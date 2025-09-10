using UnityEngine;

/// <summary>
/// 경험치 오브젝트. PlayerExpCollector가 흡수.
/// </summary>
[DisallowMultipleComponent]
public sealed class ExpOrb : MonoBehaviour
{
    public int value = 1;
    private PooledObject p;

    void Awake() => p = GetComponent<PooledObject>();

    public void MoveTowards(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void ReturnToPool()
    {
        if (p != null) p.Return();
        else gameObject.SetActive(false);
    }
}
