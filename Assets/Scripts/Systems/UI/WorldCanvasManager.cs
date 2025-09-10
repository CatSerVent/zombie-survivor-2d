using UnityEngine;

/// <summary>
/// DamageText 등 월드 좌표에 표시되는 UI의 부모.
/// </summary>
[DisallowMultipleComponent]
public sealed class WorldCanvasManager : MonoBehaviour
{
    public static Transform I;

    void Awake() => I = transform;
}
