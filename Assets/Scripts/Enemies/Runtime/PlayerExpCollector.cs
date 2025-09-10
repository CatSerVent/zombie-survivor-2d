using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerExpCollector : MonoBehaviour
{
    [SerializeField] private float baseRadius = 0.8f;
    [SerializeField] private LayerMask orbMask;
    [SerializeField] private LevelSystem levelSystem;

    private readonly Collider2D[] buffer = new Collider2D[16];

    [HideInInspector] public float magnetMul = 1f; // ✅ 배수로 반영

    void Update()
    {
        float radius = baseRadius * magnetMul;
        int n = Physics2D.OverlapCircleNonAlloc(transform.position, radius, buffer, orbMask);
        for (int i = 0; i < n; i++)
        {
            var orb = buffer[i].GetComponent<ExpOrb>();
            if (orb != null)
            {
                orb.Absorb(transform, (value) =>
                {
                    if (levelSystem != null)
                        levelSystem.AddExp(value);
                });
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, baseRadius * magnetMul);
    }
}
