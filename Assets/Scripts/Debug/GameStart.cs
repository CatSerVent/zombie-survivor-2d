using UnityEngine;

public class GameStart : MonoBehaviour
{
    public WeaponController weapons;     // Player에 있는 WeaponController
    public WeaponData startWeapon;       // Inspector에서 "돌격소총(SingleShot)" ScriptableObject 연결

    void Start()
    {
        if (weapons != null && startWeapon != null)
        {
            weapons.AddWeapon(startWeapon);
            Debug.Log("기본 무기 지급: " + startWeapon.displayName);
        }
    }
}
