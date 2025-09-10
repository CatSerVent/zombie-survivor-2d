using Game.Common;
using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class PerfHUD : MonoBehaviour
{
    [Header("Texts")]
    public Text fpsText;
    public Text memText;
    public Text waveText;
    public Text aliveText;

    [Tooltip("Exp 현재값을 표시할 Text (선택). showExpProgress=true면 현재/다음 형식으로 표시합니다.")]
    public Text expText;

    [Tooltip("경험치 배수를 별도로 표시할 Text (선택). 비어있으면 Wave 라인 뒤에 x배수로 붙여서 표시합니다.")]
    public Text expMultText;

    [Header("References (Optional)")]
    public LevelSystem level;   // curExp/nextExp 읽기
    public WaveManager waveMgr; // 필요 시 Wave 정보 직접 읽기

    [Header("Options")]
    [Range(0.05f, 0.5f)]
    public float refreshInterval = 0.2f; // UI 업데이트 주기(저주기)
    public bool showExpProgress = false; // true: "Exp cur/next", false: "Exp cur"

    // ---- 내부 캐시(변화 있을 때만 문자열 갱신 → GC 최소화) ----
    float accTime;
    int accFrames;
    int _fps = -1, _lastFps = -2;
    int _lastMem = -1;

    int _lastWave = -1;
    int _lastAlive = -1;

    int _lastExpCur = int.MinValue;
    int _lastExpNext = int.MinValue;

    private float _lastExpMul = 1f;

    bool aliveHooked = false;

    void Reset()
    {
        if (level == null) level = FindObjectOfType<LevelSystem>(includeInactive: true);
        if (waveMgr == null) waveMgr = FindObjectOfType<WaveManager>(includeInactive: true);
    }

    void Update()
    {
        // FPS 샘플링 (0.5초 단위 평균)
        accFrames++;
        accTime += Time.unscaledDeltaTime;
        if (accTime >= 0.5f)
        {
            _fps = Mathf.RoundToInt(accFrames / accTime);
            accTime = 0f;
            accFrames = 0;
        }

        // UI 저주기 갱신
        _uiTick += Time.unscaledDeltaTime;
        if (_uiTick < refreshInterval) return;
        _uiTick = 0f;

        UpdateFps();
        UpdateMem();
        UpdateWaveAndMult();
        UpdateAlive();
        UpdateExp(); // ← 추가
        TryHookAlive(); // EnemyCounter 생성 타이밍 대응
    }

    float _uiTick;

    void UpdateFps()
    {
        if (fpsText == null) return;
        if (_fps != _lastFps)
        {
            fpsText.text = $"{_fps} FPS";
            _lastFps = _fps;
        }
    }

    void UpdateMem()
    {
        if (memText == null) return;
        long bytes = Profiler.GetTotalAllocatedMemoryLong();
        int mb = Mathf.RoundToInt(bytes / (1024f * 1024f));
        if (mb != _lastMem)
        {
            memText.text = $"{mb}.0MB";
            _lastMem = mb;
        }
    }

    void UpdateWaveAndMult()
    {
        //if (waveText == null) return;

        //int curWave = WaveManager.CurrentWave;
        //if (curWave <= 0 && waveMgr != null)
        //{
        //    var f = typeof(WaveManager).GetField("waveIndex",
        //        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //    if (f != null) curWave = Mathf.Max(curWave, (int)f.GetValue(waveMgr));
        //}

        //bool waveChanged = (curWave != _lastWave);
        //if (waveChanged) _lastWave = curWave;

        //// ★ 10의 배수에서 변경되도록 정확히 계산
        //float calcMult = ComputeExpMultiplier(Mathf.Max(1, _lastWave));
        //bool multChanged = !Mathf.Approximately(calcMult, _lastExpMul);
        //if (multChanged) _lastExpMul = calcMult;

        //if (waveChanged || multChanged) ApplyWaveAndMultiplierToText();

        float calcMult = ExpMath.ComputeExpMultiplier(Mathf.Max(1, _lastWave));
    }

    void ApplyWaveAndMultiplierToText()
    {
        if (waveText == null) return;

        string waveStr = (_lastWave > 0) ? $"Wave {_lastWave}" : "Wave -";

        // ★ 1.0은 반드시 한 자리 소수 표시, 그 외는 0.0## (한 자리 이상, 최대 3자리)
        string multStr = (_lastExpMul == 1f) ? "x1.0" : $"x{_lastExpMul:0.0##}";

        if (expMultText != null)
        {
            waveText.text = waveStr;
            expMultText.text = multStr;
        }
        else
        {
            waveText.text = $"{waveStr}  {multStr}";
        }
    }

    void UpdateAlive()
    {
        if (aliveText == null) return;

        int alive = EnemyCounter.I != null ? EnemyCounter.I.Alive : _lastAlive;
        if (alive != _lastAlive)
        {
            _lastAlive = alive;
            aliveText.text = $"Alive {alive}";
        }
    }

    void TryHookAlive()
    {
        if (aliveHooked) return;
        if (EnemyCounter.I == null) return;

        EnemyCounter.I.OnAliveChanged += OnAliveChanged;
        aliveHooked = true;
        // 초기값 즉시 반영
        OnAliveChanged(EnemyCounter.I.Alive);
    }

    void OnAliveChanged(int v)
    {
        _lastAlive = v;
        if (aliveText != null) aliveText.text = $"Alive {v}";
    }

    void UpdateExp()
    {
        if (expText == null || level == null) return;

        int cur = level.curExp;
        int next = level.nextExp;

        if (showExpProgress)
        {
            if (cur != _lastExpCur || next != _lastExpNext)
            {
                expText.text = $"Exp {cur}/{next}";
                _lastExpCur = cur;
                _lastExpNext = next;
            }
        }
        else
        {
            if (cur != _lastExpCur)
            {
                expText.text = $"Exp {cur}";
                _lastExpCur = cur;
                _lastExpNext = next; // 일관성 유지
            }
        }
    }

    // ---- 외부에서 호출되는 메서드들(기존 호환) ----

    public void SetWave(int waveIndex)
    {
        if (waveIndex != _lastWave)
        {
            _lastWave = waveIndex;
            ApplyWaveAndMultiplierToText();
        }
    }

    public void SetAlive(int alive)
    {
        _lastAlive = alive;
        if (aliveText != null) aliveText.text = $"Alive {alive}";
    }

    public void SetExpMultiplier(float multiplier)
    {
        _lastExpMul = multiplier;
        ApplyWaveAndMultiplierToText(); // ← 항상 x1.0 포함해 갱신
    }

    private static float ComputeExpMultiplier(int waveIndex)
    {
        if (waveIndex < 1) return 1f;
        int steps = waveIndex / 10;          // 1~9:0, 10~19:1, 20~29:2, ...
        return Mathf.Pow(1.5f, steps);       // 1.0, 1.5, 2.25, 3.375, ...
    }
}
