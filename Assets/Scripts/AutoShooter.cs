using UnityEngine;
using Game.Systems;

public class AutoShooter : MonoBehaviour
{
    //public float fireRate = 0.25f;
    //public float bulletSpeed = 12f;
    //public float range = 12f;
    ////public ObjectPool bulletPool;
    //float t;
    //void Update()
    //{
    //    t += Time.deltaTime;
    //    if (t < fireRate) return;
    //    var target = FindNearestEnemy();
    //    if (target == null) return;
    //    t = 0f;
    //    var dir = (target.position - transform.position).normalized;
    //    var b = bulletPool.Get();
    //    b.transform.position = transform.position;
    //    var rb = b.GetComponent<Rigidbody2D>();
    //    if (rb == null) rb = b.AddComponent<Rigidbody2D>();
    //    rb.gravityScale = 0f;
    //    rb.velocity = dir * bulletSpeed;
    //}
    //Transform FindNearestEnemy()
    //{
    //    var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    if (enemies.Length == 0) return null;
    //    var me = transform.position;
    //    Transform best = null; float bestD = range;
    //    foreach (var e in enemies)
    //    {
    //        float d = Vector3.Distance(me, e.transform.position);
    //        if (d < bestD) { bestD = d; best = e.transform; }
    //    }
    //    return best;
    //}
}