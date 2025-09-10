using UnityEngine;

/// <summary>
/// WeaponData의 인스턴스화 — 현재 레벨/쿨다운 등을 관리.
/// </summary>
[System.Serializable]
public class WeaponInstance
{
    public WeaponData data;
    public int level = 1;
    public float lastFireTime = -999f;

    public WeaponInstance(WeaponData data)
    {
        this.data = data;
        this.level = 1;
    }

    public float Cooldown()
    {
        return 1f / (data.fireRate * SpeedMul());
    }

    public float Damage(float globalMul)
    {
        return data.baseDamage * DamageMul() * globalMul;
    }

    float DamageMul()
    {
        if (data.damageMultipliers != null && data.damageMultipliers.Length >= level)
            return data.damageMultipliers[level - 1];
        return 1f;
    }

    float SpeedMul()
    {
        if (data.speedMultipliers != null && data.speedMultipliers.Length >= level)
            return data.speedMultipliers[level - 1];
        return 1f;
    }

    public float Speed() => data.range; // 발사 속도 대신 이동 속도로 쓰던 부분
    public int Pierce() => 1;
    public float SplashRadius() => 0f;
    public float SplashRatio() => 0f;
    public int ProjectileCount() => Mathf.Min(5, level);
}
