using UnityEngine;

/// <summary>
/// DamageText �� ���� ��ǥ�� ǥ�õǴ� UI�� �θ�.
/// </summary>
[DisallowMultipleComponent]
public sealed class WorldCanvasManager : MonoBehaviour
{
    public static Transform I;

    void Awake() => I = transform;
}
