using UnityEngine;

public class GameStart : MonoBehaviour
{
    public WeaponController weapons;     // Player�� �ִ� WeaponController
    public WeaponData startWeapon;       // Inspector���� "���ݼ���(SingleShot)" ScriptableObject ����

    void Start()
    {
        if (weapons != null && startWeapon != null)
        {
            weapons.AddWeapon(startWeapon);
            Debug.Log("�⺻ ���� ����: " + startWeapon.displayName);
        }
    }
}
