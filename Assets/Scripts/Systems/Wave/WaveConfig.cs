using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ED/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public int baseCount = 10;

    public int perWaveIncOdd = 1;   // 1웨이브 이후 홀수 웨이브마다 +1
    public int perWaveIncEven = 2;  // 짝수 웨이브마다 +2
    public int bonusPer10 = 10;     // 10웨이브마다 +10

    public float baseInterval = 0.9f;
    public AnimationCurve intervalScale;

    public int maxAlive = 80;
    public int spawnBatch = 3;
    public float batchInterval = 0.20f;
}
