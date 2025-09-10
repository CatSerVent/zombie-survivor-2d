using Game.Enemies.Runtime;
using System.Collections;
using UnityEngine;


public class WaveManager : MonoBehaviour
{
    public Transform player;
    public float radius = 12f;
    public float waveDelay = 7f;
    public PerfHUD hud;
    public WaveConfig config;
    public SpawnTable table;

    int waveIndex = 0;

    public static int CurrentWave { get; private set; }
    public static float ExpMultiplier { get; private set; }

    void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        while (true)
        {
            waveIndex++;
            CurrentWave = waveIndex;
            ExpMultiplier = Mathf.Pow(1.5f, waveIndex / 10);

            if (hud) hud.SetWave(waveIndex);

            int targetCount = CalcWaveCount(waveIndex);
            int spawned = 0;

            while (spawned < targetCount)
            {
                while ((EnemyCounter.I?.Alive ?? 0) >= config.maxAlive) yield return null;

                int batch = Mathf.Min(config.spawnBatch, targetCount - spawned);
                for (int i = 0; i < batch; i++)
                {
                    var data = PickEnemyData(waveIndex);
                    Vector2 r = Random.insideUnitCircle.normalized * radius;
                    Vector3 pos = player.position + new Vector3(r.x, r.y, 0f);
                    var e = Instantiate(data.prefab, pos, Quaternion.identity);

                    float hpK = table.hpScale?.Evaluate(waveIndex) ?? 1f;
                    float spK = table.speedScale?.Evaluate(waveIndex) ?? 1f;

                    var enemy = e.GetComponent<Enemy>();
                    if (enemy != null) enemy.Setup(Mathf.Max(1, Mathf.RoundToInt(data.baseHp * hpK)));

                    var ai = e.GetComponent<EnemyAI>();
                    if (ai != null) ai.SetupSpeed(data.baseSpeed * spK);

                    spawned++;
                    if (spawned >= targetCount) break;
                }

                float s = config.intervalScale?.Evaluate(waveIndex) ?? 1f;
                float interval = Mathf.Max(0.05f, config.baseInterval * s);
                yield return new WaitForSeconds(config.batchInterval > 0f ? config.batchInterval : interval);
            }

            yield return new WaitForSeconds(waveDelay);
        }
    }

    int CalcWaveCount(int wave)
    {
        if (wave <= 1) return Mathf.Max(1, config.baseCount);

        int prev = wave - 1;
        int evenCount = prev / 2;
        int oddCount = prev - evenCount;

        int inc = oddCount * config.perWaveIncOdd + evenCount * config.perWaveIncEven;
        int bonus10 = (wave / 10) * config.bonusPer10;

        int count = config.baseCount + inc + bonus10;
        return Mathf.Max(1, count);
    }

    EnemyData PickEnemyData(int wave)
    {
        if (wave < 5) return table.normal;

        if (wave < 10)
        {
            float rn = table.ratioNormal?.Evaluate(wave) ?? 1f;
            float rr = table.ratioRunner?.Evaluate(wave) ?? 0f;
            float sum = rn + rr;
            if (sum <= 0f) return table.normal;
            float r = Random.value * sum;
            if (r < rn) return table.normal;
            return table.runner;
        }

        float rN = table.ratioNormal?.Evaluate(wave) ?? 1f;
        float rR = table.ratioRunner?.Evaluate(wave) ?? 0f;
        float rT = table.ratioTank?.Evaluate(wave) ?? 0f;
        float sumAll = rN + rR + rT;
        if (sumAll <= 0f) return table.normal;
        float rv = Random.value * sumAll;
        if (rv < rN) return table.normal; rv -= rN;
        if (rv < rR) return table.runner;
        return table.tank;
    }
}
