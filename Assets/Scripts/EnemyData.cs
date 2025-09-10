using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ED/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string id;
    public GameObject prefab;
    public int baseHp;
    public float baseSpeed;
}
