using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ED/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public int maxLevel = 5;

    public float baseDamage = 10f;
    public float baseSpeed = 12f;
    public float range = 10f;
    public float baseFireRate = 1f;   // √ ¥Á πﬂªÁ »Ωºˆ (ex. µπ∞›º“√—=4, º¶∞«=1, ø¿∫Ò∆Æ=0.5)

    public enum WeaponKind { SingleShot, Shotgun, Orbit }
    public WeaponKind kind;

    public int[] piercePerLevel;
    public float[] splashRadiusPerLevel;
    public float[] splashRatioPerLevel;
}
