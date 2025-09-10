using UnityEngine;
using Game.Systems;

public class DamageTextPoolManager : MonoBehaviour
{
    public static DamageTextPoolManager I;
    public ObjectPool<DamageText> pool;

    [SerializeField] private DamageText prefab;
    [SerializeField] private int initialCapacity = 20;
    [SerializeField] private int maxSize = 50;

    void Awake()
    {
        I = this;
        pool = new ObjectPool<DamageText>(prefab, initialCapacity, maxSize, this.transform);
    }
}
