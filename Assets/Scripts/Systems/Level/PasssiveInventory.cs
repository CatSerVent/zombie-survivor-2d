using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 패시브 아이템(이동속도, 데미지, 경험치, 자석)을 관리.
/// 레벨업 시 각 패시브의 수치를 적용.
/// </summary>
[DisallowMultipleComponent]
public sealed class PassiveInventory : MonoBehaviour
{
    [Header("연결 컴포넌트")]
    public PlayerController player;
    public LevelSystem levelSys;
    public WeaponController weapons;
    public PlayerExpCollector collector;

    // 타입별 현재 레벨 저장
    private Dictionary<PassiveType, int> levels = new Dictionary<PassiveType, int>()
    {
        { PassiveType.MoveSpeed, 0 },
        { PassiveType.Damage,    0 },
        { PassiveType.ExpGain,   0 },
        { PassiveType.Magnet,    0 },
    };

    // 타입별 현재 적용된 value 기록
    private Dictionary<PassiveType, float> appliedValues = new Dictionary<PassiveType, float>()
    {
        { PassiveType.MoveSpeed, 0f },
        { PassiveType.Damage,    0f },
        { PassiveType.ExpGain,   0f },
        { PassiveType.Magnet,    0f },
    };

    public int GetLevel(PassiveType type) =>
        levels.TryGetValue(type, out var lv) ? lv : 0;

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
                if (collector != null) collector.SetMagnetMultiplier(1f + newV);
                break;
        }

        levels[p.type] = newLevel;
        appliedValues[p.type] = newV;
    }
}
