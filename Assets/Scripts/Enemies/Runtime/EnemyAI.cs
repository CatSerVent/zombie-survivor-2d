using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2.8f;
    Transform player;

    public void SetupSpeed(float s) { speed = s; }

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
}
