using UnityEngine;

public enum PassiveType { MoveSpeed, Damage, ExpGain, Magnet}

[CreateAssetMenu(fileName = "PassiveData", menuName = "ED/PassiveData")]
public class PassiveData : ScriptableObject
{
    public string id;              // 고유 ID
    public string title;           // UI 표시용
    public int maxLevel = 5;

    public PassiveType type;       // 어떤 종류의 패시브인지
    public float[] values;         // 레벨별 수치 (예: [0.1f,0.2f,0.3f,...] -> 10%,20%,30%...)
}
