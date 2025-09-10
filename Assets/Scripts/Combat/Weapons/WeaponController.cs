using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 플레이어 무기 컨트롤러.
/// 무기별 발사 로직(SingleShot, Shotgun, Orbit)을 처리.
/// </summary>
[DisallowMultipleComponent]
public sealed class WeaponController : MonoBehaviour
{
    [Header("플레이어 Transform")]
    [Tooltip("총알 발사 위치 기준이 될 Transform")]
    public Transform player;

    [Header("총알 풀")]
    [Tooltip("발사할 BulletHitPierce 풀")]
    public ObjectPool<BulletHitPierce> bulletPool;

    [Header("데미지 배수 (패시브 연동)")]
    public float damageMul = 1f;

    public List<WeaponInstance> weapons = new List<WeaponInstance>();
    private float orbitAngle;

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
        BulletHitPierce go = bulletPool.Get();
        if (go == null) return;

        go.transform.position = pos;
        go.transform.rotation = Quaternion.identity;

        var rb = go.GetComponent<Rigidbody2D>();
        if (!rb) rb = go.gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.velocity = dir * inst.Speed();

        go.damage = Mathf.CeilToInt(inst.Damage(damageMul));
        go.pierce = inst.Pierce();
        go.splashRadius = inst.SplashRadius();
        go.splashRatio = inst.SplashRatio();

        go.gameObject.SetActive(true);
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
