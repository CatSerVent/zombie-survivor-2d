using UnityEngine;

/// <summary>
/// 웨이브 지수에 따라 경험치 배수를 계산해 PerfHUD로 전달.
/// - 규칙: 매 10웨이브마다 ×1.5 배수 증가 (1~9:1.0, 10~19:1.5, 20~29:2.25, ...).
/// - WaveManager에 이벤트가 없으면 저주기 폴링으로 변경 감지.
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
        Push(); // 시작 시 x1.0 표시
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
        int w = WaveManager.CurrentWave; // 프로젝트에 있는 정적 프로퍼티 사용
        if (w <= 0 && wave != null)
        {
            // 방어적: 내부값이 인스펙터/초기화 타이밍상 아직 0일 수도 있음
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
        // ★ 배수 계산: 10웨이브마다 ×1.5
        int steps = (waveIndex - 1) / 10;      // 1~9 -> 0, 10~19 -> 1, ...
        float mult = Mathf.Pow(1.5f, steps);   // 1.0, 1.5, 2.25, ...
        hud.SetExpMultiplier(mult);
    }

    // WaveManager에서 콜백을 제공한다면, 거기에 연결해서 바로 호출 가능:
    public void OnWaveStarted(int waveIndex)
    {
        _lastWave = waveIndex;
        int steps = (waveIndex - 1) / 10;
        float mult = Mathf.Pow(1.5f, steps);
        if (hud != null) hud.SetExpMultiplier(mult);
    }
}
