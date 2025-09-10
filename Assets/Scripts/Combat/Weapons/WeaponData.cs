using UnityEngine;

/// <summary>
/// 무기 데이터(SO) — 발사 속도, 데미지, 사거리, 종류 등을 정의.
/// </summary>
[CreateAssetMenu(menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public enum WeaponKind { SingleShot, Shotgun, Orbit }

    [Header("기본 속성")]
    [Tooltip("무기 종류")]
    public WeaponKind kind;

    [Tooltip("최대 레벨")]
    public int maxLevel = 5;

    [Tooltip("기본 사거리")]
    public float range = 5f;

    [Tooltip("기본 데미지")]
    public int baseDamage = 1;

    [Tooltip("기본 발사 속도")]
    public float fireRate = 1f;

    [Header("레벨별 스탯")]
    [Tooltip("레벨별 데미지 배율")]
    public float[] damageMultipliers;

    [Tooltip("레벨별 발사속도 배율")]
    public float[] speedMultipliers;
}
