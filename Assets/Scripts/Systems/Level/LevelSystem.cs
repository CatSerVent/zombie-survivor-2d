using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public int level = 1;
    public int curExp = 0;
    public int nextExp = 5;

    public float expGainMul = 1f;

    public List<WeaponData> allWeapons;
    public List<PassiveData> allPassives;

    public LevelUpUI ui;
    public WeaponController weapons;
    public PassiveInventory passives;

    List<LevelUpOption> curOptions;
    bool canReroll;

    public void AddExp(int v)
    {
        int add = Mathf.Max(0, Mathf.RoundToInt(v * Mathf.Max(0.01f, expGainMul)));
        curExp += add;
        if (curExp >= nextExp)
        {
            curExp -= nextExp;
            level++;
            nextExp = Mathf.CeilToInt(nextExp * 1.6f);
            OpenLevelUp();
        }
    }

    void OpenLevelUp()
    {
        curOptions = PickUpToThree();
        if (curOptions.Count == 0) return;
        canReroll = true;
        if (ui != null) ui.Open(curOptions, OnSelectOption, OnReroll, canReroll);
        if (AudioManager.I) AudioManager.I.PlayLevelUp();
        Time.timeScale = 0f;
    }

    void OnReroll()
    {
        if (!canReroll) return;
        curOptions = PickUpToThree();
        canReroll = false;
        if (ui != null) ui.Refresh(curOptions, canReroll);
        Time.timeScale = 0f;
    }

    List<LevelUpOption> PickUpToThree()
    {
        List<LevelUpOption> candidates = new List<LevelUpOption>();

        // ���� �ĺ�
        for (int i = 0; i < allWeapons.Count; i++)
        {
            var w = allWeapons[i];
            int cur = GetWeaponLevel(w);
            if (cur < w.maxLevel)
            {
                LevelUpOption o = new LevelUpOption();
                o.isWeapon = true;
                o.weapon = w;
                o.passive = null;
                string name = string.IsNullOrEmpty(w.displayName) ? w.id : w.displayName;
                o.title = cur > 0 ? name + " Lv." + (cur + 1) : name;
                o.subtitle = DescribeWeapon(w, cur + 1);
                candidates.Add(o);
            }
        }

        // �нú� �ĺ� (�� ����: id ��� �� Ÿ�� ��� ��ȸ)
        for (int i = 0; i < allPassives.Count; i++)
        {
            var p = allPassives[i];
            int cur = 0;
            if (passives != null)
            {
                // PassiveInventory�� Ÿ�Ժ��� ���� ������ �����ϵ��� ����Ǿ���
                cur = passives.GetLevel(p.type);
            }

            if (cur < p.maxLevel)
            {
                LevelUpOption o = new LevelUpOption();
                o.isWeapon = false;
                o.weapon = null;
                o.passive = p;
                o.title = p.title + " Lv." + (cur + 1);
                o.subtitle = DescribePassive(p, cur + 1);
                candidates.Add(o);
            }
        }

        // ���� ����
        for (int i = 0; i < candidates.Count; i++)
        {
            int j = Random.Range(i, candidates.Count);
            var t = candidates[i]; candidates[i] = candidates[j]; candidates[j] = t;
        }

        int k = candidates.Count >= 3 ? 3 : candidates.Count;
        List<LevelUpOption> picks = new List<LevelUpOption>(k);
        for (int i = 0; i < k; i++) picks.Add(candidates[i]);
        return picks;
    }

    string DescribeWeapon(WeaponData w, int nextLv)
    {
        return w.displayName + " ������ �� Lv." + nextLv;
    }

    string DescribePassive(PassiveData p, int nextLv)
    {
        float v = 0f;
        if (p.values != null && p.values.Length > 0)
        {
            int idx = Mathf.Clamp(nextLv - 1, 0, p.values.Length - 1);
            v = p.values[idx];
        }
        int pct = Mathf.RoundToInt(v * 100f);
        switch (p.type)
        {
            case PassiveType.MoveSpeed: return "�̵��ӵ� +" + pct + "%";
            case PassiveType.Damage: return "������ +" + pct + "%";
            case PassiveType.ExpGain: return "����ġ +" + pct + "%";
        }
        return "";
    }

    void OnSelectOption(LevelUpOption o)
    {
        if (o.isWeapon)
        {
            if (weapons != null && o.weapon != null) weapons.AddWeapon(o.weapon);
        }
        else
        {
            if (passives != null && o.passive != null) passives.Apply(o.passive);
        }
    }

    // ���� ���� ���� ��������
    int GetWeaponLevel(WeaponData data)
    {
        if (weapons == null) return 0;
        foreach (var inst in weapons.GetAllWeapons())
        {
            if (inst.data == data) return inst.level;
        }
        return 0;
    }
}
