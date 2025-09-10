using UnityEngine;

public class WeaponInstance
{
    public WeaponData data;
    public int level;
    public float lastFireTime;

    public WeaponInstance(WeaponData d)
    {
        data = d;
        level = 1;
        lastFireTime = -999f;
    }

    public float Damage(float mul = 1f)
    {
        return data.baseDamage * (1f + 0.2f * (level - 1)) * mul;
    }

    public float Speed()
    {
        return data.baseSpeed;
    }

    public int ProjectileCount()
    {
        return 1 + (level / 3);
    }

    public int Pierce()
    {
        if (data.piercePerLevel == null || data.piercePerLevel.Length == 0) return 1;
        int i = Mathf.Clamp(level - 1, 0, data.piercePerLevel.Length - 1);
        return Mathf.Max(1, data.piercePerLevel[i]);
    }

    public float SplashRadius()
    {
        if (data.splashRadiusPerLevel == null || data.splashRadiusPerLevel.Length == 0) return 0f;
        int i = Mathf.Clamp(level - 1, 0, data.splashRadiusPerLevel.Length - 1);
        return Mathf.Max(0f, data.splashRadiusPerLevel[i]);
    }

    public float SplashRatio()
    {
        if (data.splashRatioPerLevel == null || data.splashRatioPerLevel.Length == 0) return 0f;
        int i = Mathf.Clamp(level - 1, 0, data.splashRatioPerLevel.Length - 1);
        return Mathf.Clamp01(data.splashRatioPerLevel[i]);
    }

    public float Cooldown()
    {
        float rate = data.baseFireRate * (1f + 0.05f * (level - 1));
        return 1f / Mathf.Max(0.01f, rate);
    }
}
