using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ED/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public int baseCount = 10;

    public int perWaveIncOdd = 1;   // 1���̺� ���� Ȧ�� ���̺긶�� +1
    public int perWaveIncEven = 2;  // ¦�� ���̺긶�� +2
    public int bonusPer10 = 10;     // 10���̺긶�� +10

    public float baseInterval = 0.9f;
    public AnimationCurve intervalScale;

    public int maxAlive = 80;
    public int spawnBatch = 3;
    public float batchInterval = 0.20f;
}
