using UnityEngine;

/// <summary>
/// ���̺� ������ ���� ����ġ ����� ����� PerfHUD�� ����.
/// - ��Ģ: �� 10���̺긶�� ��1.5 ��� ���� (1~9:1.0, 10~19:1.5, 20~29:2.25, ...).
/// - WaveManager�� �̺�Ʈ�� ������ ���ֱ� �������� ���� ����.
/// </summary>
public sealed class WaveExpMultiplierHUDBridge : MonoBehaviour
{
    public PerfHUD hud;
    public WaveManager wave;

    [Range(0.05f, 0.5f)]
    public float checkInterval = 0.2f;

    private int _lastWave = -1;
    private float _acc;

    private void Reset()
    {
        if (hud == null) hud = FindObjectOfType<PerfHUD>(includeInactive: true);
        if (wave == null) wave = FindObjectOfType<WaveManager>(includeInactive: true);
    }

    private void Start()
    {
        Push(); // ���� �� x1.0 ǥ��
    }

    private void Update()
    {
        _acc += Time.unscaledDeltaTime;
        if (_acc < checkInterval) return;
        _acc = 0f;

        int cur = GetCurrentWave();
        if (cur != _lastWave)
        {
            _lastWave = cur;
            Push();
        }
    }

    private int GetCurrentWave()
    {
        int w = WaveManager.CurrentWave; // ������Ʈ�� �ִ� ���� ������Ƽ ���
        if (w <= 0 && wave != null)
        {
            // �����: ���ΰ��� �ν�����/�ʱ�ȭ Ÿ�ֻ̹� ���� 0�� ���� ����
            var f = typeof(WaveManager).GetField("waveIndex",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (f != null) w = Mathf.Max(w, (int)f.GetValue(wave));
        }
        return w;
    }

    private void Push()
    {
        if (hud == null) return;

        int waveIndex = Mathf.Max(1, GetCurrentWave());
        // �� ��� ���: 10���̺긶�� ��1.5
        int steps = (waveIndex - 1) / 10;      // 1~9 -> 0, 10~19 -> 1, ...
        float mult = Mathf.Pow(1.5f, steps);   // 1.0, 1.5, 2.25, ...
        hud.SetExpMultiplier(mult);
    }

    // WaveManager���� �ݹ��� �����Ѵٸ�, �ű⿡ �����ؼ� �ٷ� ȣ�� ����:
    public void OnWaveStarted(int waveIndex)
    {
        _lastWave = waveIndex;
        int steps = (waveIndex - 1) / 10;
        float mult = Mathf.Pow(1.5f, steps);
        if (hud != null) hud.SetExpMultiplier(mult);
    }
}
