using UnityEngine;

public enum PassiveType { MoveSpeed, Damage, ExpGain, Magnet}

[CreateAssetMenu(fileName = "PassiveData", menuName = "ED/PassiveData")]
public class PassiveData : ScriptableObject
{
    public string id;              // ���� ID
    public string title;           // UI ǥ�ÿ�
    public int maxLevel = 5;

    public PassiveType type;       // � ������ �нú�����
    public float[] values;         // ������ ��ġ (��: [0.1f,0.2f,0.3f,...] -> 10%,20%,30%...)
}
