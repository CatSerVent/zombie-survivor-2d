using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// URP 2D 품질 프리셋 토글러
/// - Low: MSAA Off, HDR Off, PP Off
/// - Medium: MSAA 2x, HDR Off, PP Off
/// - High: MSAA 4x, HDR On, PP On
/// </summary>
[DisallowMultipleComponent]
public sealed class QualityPresetManager : MonoBehaviour
{
    public enum QualityPreset { Low = 0, Medium = 1, High = 2 }

    [Header("Optional: 지정하지 않으면 MainCamera 자동 탐색")]
    [SerializeField] private Camera targetCamera;

    [Header("Persistence")]
    [Tooltip("프리셋을 PlayerPrefs에 저장/로드합니다.")]
    [SerializeField] private bool savePresetToPlayerPrefs = true;

    [Tooltip("PlayerPrefs 키")]
    [SerializeField] private string playerPrefsKey = "QualityPreset";

    private UniversalAdditionalCameraData camData;
    private UniversalRenderPipelineAsset urp;
    private QualityPreset lastApplied = QualityPreset.Low;

    void Awake()
    {
        urp = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (targetCamera == null)
            targetCamera = Camera.main;
        if (targetCamera != null)
            camData = targetCamera.GetUniversalAdditionalCameraData();

        if (savePresetToPlayerPrefs && PlayerPrefs.HasKey(playerPrefsKey))
        {
            int idx = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey, 0), 0, 2);
            Apply((QualityPreset)idx);
        }
        else Apply(QualityPreset.Low);
    }

    public void ApplyLow() => Apply(QualityPreset.Low);
    public void ApplyMedium() => Apply(QualityPreset.Medium);
    public void ApplyHigh() => Apply(QualityPreset.High);

    public void Apply(QualityPreset preset)
    {
        if (urp == null) return;
        switch (preset)
        {
            case QualityPreset.Low:
                SetMsaa(0); SetHdr(false); SetPostProcessing(false); break;
            case QualityPreset.Medium:
                SetMsaa(2); SetHdr(false); SetPostProcessing(false); break;
            case QualityPreset.High:
                SetMsaa(4); SetHdr(true); SetPostProcessing(true); break;
        }
        lastApplied = preset;
        if (savePresetToPlayerPrefs)
            PlayerPrefs.SetInt(playerPrefsKey, (int)preset);
    }

    void SetMsaa(int samples) => urp.msaaSampleCount = Mathf.Clamp(samples, 0, 8);
    void SetHdr(bool enabled) => urp.supportsHDR = enabled;

    void SetPostProcessing(bool enabled)
    {
        if (camData == null)
        {
            if (targetCamera == null) targetCamera = Camera.main;
            if (targetCamera != null)
                camData = targetCamera.GetUniversalAdditionalCameraData();
        }
        if (camData != null)
            camData.renderPostProcessing = enabled;
    }
}
