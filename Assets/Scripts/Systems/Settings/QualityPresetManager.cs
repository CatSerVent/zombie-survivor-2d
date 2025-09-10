using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Systems.Settings
{
    public enum QualityPreset { Low = 0, Medium = 1, High = 2 }

    /// <summary>
    /// URP 2D 품질 프리셋 토글러
    /// - Low:   MSAA Off, HDR Off,  PP Off
    /// - Medium:MSAA 2x,  HDR Off,  PP Off
    /// - High:  MSAA 4x,  HDR On,   PP On
    /// 근거: 컨텍스트의 P0 "URP MSAA/HDR/PP 최소" 요구와 모바일 성능 우선 원칙.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class QualityPresetManager : MonoBehaviour
    {
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

        private void Awake()
        {
            urp = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

            if (targetCamera == null)
                targetCamera = Camera.main;

            if (targetCamera != null)
                camData = targetCamera.GetUniversalAdditionalCameraData();

            // 저장된 프리셋 로드
            if (savePresetToPlayerPrefs && PlayerPrefs.HasKey(playerPrefsKey))
            {
                int idx = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey, 0), 0, 2);
                Apply((QualityPreset)idx);
            }
            else
            {
                // 기본값: Medium 정도가 무난하지만, 컨텍스트 기본은 "최소"이므로 Low부터.
                Apply(QualityPreset.Low);
            }
        }

        /// <summary>UI 버튼용 래퍼</summary>
        public void ApplyLow() => Apply(QualityPreset.Low);
        public void ApplyMedium() => Apply(QualityPreset.Medium);
        public void ApplyHigh() => Apply(QualityPreset.High);

        /// <summary>Dropdown(int index)용 래퍼: 0=Low,1=Medium,2=High</summary>
        public void ApplyByIndex(int index)
        {
            Apply((QualityPreset)Mathf.Clamp(index, 0, 2));
        }

        public void Apply(QualityPreset preset)
        {
            if (urp == null)
            {
                Debug.LogWarning("[QualityPresetManager] URP Asset이 없습니다. Project Settings > Graphics 확인");
                return;
            }

            switch (preset)
            {
                case QualityPreset.Low:
                    SetMsaa(0);
                    SetHdr(false);
                    SetPostProcessing(false);
                    break;

                case QualityPreset.Medium:
                    SetMsaa(2);
                    SetHdr(false);
                    SetPostProcessing(false);
                    break;

                case QualityPreset.High:
                    SetMsaa(4);
                    SetHdr(true);
                    SetPostProcessing(true);
                    break;
            }

            lastApplied = preset;

            if (savePresetToPlayerPrefs)
                PlayerPrefs.SetInt(playerPrefsKey, (int)preset);
        }

        private void SetMsaa(int samples)
        {
            urp.msaaSampleCount = Mathf.Clamp(samples, 0, 8);
        }

        private void SetHdr(bool enabled)
        {
            // 2022.3 LTS의 URP Asset API
            urp.supportsHDR = enabled;
        }

        private void SetPostProcessing(bool enabled)
        {
            // 대상 카메라 재확보
            if (camData == null)
            {
                if (targetCamera == null) targetCamera = Camera.main;
                if (targetCamera != null)
                    camData = targetCamera.GetUniversalAdditionalCameraData();
            }

            if (camData != null)
                camData.renderPostProcessing = enabled;

            // 필요시 씬의 모든 카메라에 적용하려면 아래 주석 해제
            // foreach (var cam in Camera.allCameras)
            // {
            //     var data = cam.GetUniversalAdditionalCameraData();
            //     if (data != null) data.renderPostProcessing = enabled;
            // }
        }

        // 디버깅용 현재 프리셋 조회
        public QualityPreset GetCurrentPreset() => lastApplied;
    }

    internal static class CameraExt
    {
        // UnityEngine.Rendering.Universal 내 확장 메서드를 감싸 호출(가독성 목적)
        public static UniversalAdditionalCameraData GetUniversalAdditionalCameraData(this Camera cam)
        {
            return cam != null ? cam.GetComponent<UniversalAdditionalCameraData>() : null;
        }
    }
}
