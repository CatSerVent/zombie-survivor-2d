using UnityEngine;
using System.Collections.Generic;
using Game.Systems;           // ✅ ObjectPool<T> 인식
using Game.Combat.Bullet;    // ✅ BulletHitPierce 인식 (BulletHitPierce.cs 네임스페이스 맞을 때만)

public class WeaponController : MonoBehaviour
{
    public Transform player;
    [SerializeField] private BulletPoolManager bulletPoolManager; // ✅ BulletPoolManager 참조

    public float damageMul = 1f;
    public List<WeaponInstance> weapons = new List<WeaponInstance>();
    float orbitAngle;

    void Update()
    {
        if (weapons.Count == 0) return;
        orbitAngle += 120f * Time.deltaTime;

        foreach (var inst in weapons)
        {
            float cd = inst.Cooldown();
            if (Time.time - inst.lastFireTime < cd) continue;

            inst.lastFireTime = Time.time;
            switch (inst.data.kind)
            {
                case WeaponData.WeaponKind.SingleShot: FireSingle(inst); break;
                case WeaponData.WeaponKind.Shotgun: FireShotgun(inst, 15f); break;
                case WeaponData.WeaponKind.Orbit: FireOrbit(inst); break;
            }
        }
    }

    public void AddWeapon(WeaponData data)
    {
        var found = weapons.Find(w => w.data == data);
        if (found != null)
        {
            if (found.level < data.maxLevel) found.level++;
        }
        else weapons.Add(new WeaponInstance(data));
    }

    public void AddOrUpgrade(WeaponData data) => AddWeapon(data);
    public List<WeaponInstance> GetAllWeapons() => weapons;

    void FireSingle(WeaponInstance inst)
    {
        var target = FindNearestEnemy(inst.data.range);
        if (target == null) return;
        var dir = (target.position - player.position).normalized;
        SpawnBullet(player.position, dir, inst);
    }

    void FireShotgun(WeaponInstance inst, float spreadDeg)
    {
        var target = FindNearestEnemy(inst.data.range);
        if (target == null) return;
        var dir = (target.position - player.position).normalized;

        int pelletCount = (inst.level >= 5) ? 5 : (inst.level >= 3) ? 4 : 3;
        float half = (pelletCount - 1) * 0.5f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = (i - half) * spreadDeg;
            Vector3 d = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
            SpawnBullet(player.position, d.normalized, inst);
        }
    }

    void FireOrbit(WeaponInstance inst)
    {
        int n = inst.ProjectileCount();
        for (int i = 0; i < n; i++)
        {
            float ang = orbitAngle + (360f / n) * i;
            Vector3 offset = new Vector3(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad), 0f) * 1.5f;
            var dir = new Vector3(-offset.y, offset.x, 0f).normalized;
            SpawnBullet(player.position + offset, dir, inst);
        }
    }
    void SpawnBullet(Vector3 pos, Vector3 dir, WeaponInstance inst)
    {
        if (bulletPoolManager == null || bulletPoolManager.Pool == null) return;

        BulletHitPierce bullet = bulletPoolManager.Pool.Get();
        if (bullet == null) return;

        bullet.gameObject.SetActive(true); // ✅ 먼저 활성화

        bullet.transform.position = pos;
        bullet.transform.rotation = Quaternion.identity;

        var rb = bullet.GetComponent<Rigidbody2D>();
        if (!rb) rb = bullet.gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;  // ✅ 움직이도록 보장
        rb.velocity = dir.normalized * inst.Speed();  // ✅ 방향 정상화

        // 총알 데이터 초기화
        bullet.damage = Mathf.CeilToInt(inst.Damage(damageMul));
        bullet.pierce = inst.Pierce();
        bullet.splashRadius = inst.SplashRadius();
        bullet.splashRatio = inst.SplashRatio();
    }


    Transform FindNearestEnemy(float range)
    {
        float best = range * range;
        Transform nearest = null;
        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float d = (e.transform.position - player.position).sqrMagnitude;
            if (d < best) { best = d; nearest = e.transform; }
        }
        return nearest;
    }
}
