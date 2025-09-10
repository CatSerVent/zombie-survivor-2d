using UnityEngine;

[CreateAssetMenu(fileName = "SpawnTable", menuName = "ED/SpawnTable")]
public class SpawnTable : ScriptableObject
{
    public EnemyData normal;
    public EnemyData runner;
    public EnemyData tank;
    public AnimationCurve ratioNormal;
    public AnimationCurve ratioRunner;
    public AnimationCurve ratioTank;
    public AnimationCurve hpScale;
    public AnimationCurve speedScale;
}
