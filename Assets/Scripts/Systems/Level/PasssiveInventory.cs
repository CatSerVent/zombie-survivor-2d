using System.Collections.Generic;
using UnityEngine;

public class PassiveInventory : MonoBehaviour
{
    public PlayerController player;
    public LevelSystem levelSys;
    public WeaponController weapons;
    public PlayerExpCollector collector;

    private Dictionary<PassiveType, int> levels = new Dictionary<PassiveType, int>()
    {
        { PassiveType.MoveSpeed, 0 },
        { PassiveType.Damage,    0 },
        { PassiveType.ExpGain,   0 },
        { PassiveType.Magnet,    0 },
    };

    private Dictionary<PassiveType, float> appliedValues = new Dictionary<PassiveType, float>()
    {
        { PassiveType.MoveSpeed, 0f },
        { PassiveType.Damage,    0f },
        { PassiveType.ExpGain,   0f },
        { PassiveType.Magnet,    0f },
    };

    public int GetLevel(PassiveType type)
    {
        return levels.TryGetValue(type, out var lv) ? lv : 0;
    }

    public bool IsMax(PassiveData p)
    {
        int cur = GetLevel(p.type);
        return cur >= p.maxLevel;
    }

    public void Apply(PassiveData p)
    {
        if (p == null) return;

        int curLevel = GetLevel(p.type);
        if (curLevel >= p.maxLevel) return;

        int newLevel = curLevel + 1;

        float oldV = appliedValues[p.type];
        float newV = 0f;
        if (p.values != null && p.values.Length > 0)
        {
            int idx = Mathf.Clamp(newLevel - 1, 0, p.values.Length - 1);
            newV = p.values[idx];
        }

        float deltaMul = (1f + newV) / Mathf.Max(1f + oldV, 1e-6f);

        switch (p.type)
        {
            case PassiveType.MoveSpeed:
                if (player != null) player.moveSpeed *= deltaMul;
                break;
            case PassiveType.Damage:
                if (weapons != null) weapons.damageMul *= deltaMul;
                break;
            case PassiveType.ExpGain:
                if (levelSys != null) levelSys.expGainMul *= deltaMul;
                break;
            case PassiveType.Magnet:
                if (collector != null) collector.magnetMul *= deltaMul;
                break;
        }

        levels[p.type] = newLevel;
        appliedValues[p.type] = newV;
    }
}
