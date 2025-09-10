using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Systems.Settings
{
    public enum QualityPreset { Low = 0, Medium = 1, High = 2 }

    /// <summary>
    /// URP 2D ǰ�� ������ ��۷�
    /// - Low:   MSAA Off, HDR Off,  PP Off
    /// - Medium:MSAA 2x,  HDR Off,  PP Off
    /// - High:  MSAA 4x,  HDR On,   PP On
    /// �ٰ�: ���ؽ�Ʈ�� P0 "URP MSAA/HDR/PP �ּ�" �䱸�� ����� ���� �켱 ��Ģ.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class QualityPresetManager : MonoBehaviour
    {
        [Header("Optional: �������� ������ MainCamera �ڵ� Ž��")]
        [SerializeField] private Camera targetCamera;

        [Header("Persistence")]
        [Tooltip("�������� PlayerPrefs�� ����/�ε��մϴ�.")]
        [SerializeField] private bool savePresetToPlayerPrefs = true;

        [Tooltip("PlayerPrefs Ű")]
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

            // ����� ������ �ε�
            if (savePresetToPlayerPrefs && PlayerPrefs.HasKey(playerPrefsKey))
            {
                int idx = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey, 0), 0, 2);
                Apply((QualityPreset)idx);
            }
            else
            {
                // �⺻��: Medium ������ ����������, ���ؽ�Ʈ �⺻�� "�ּ�"�̹Ƿ� Low����.
                Apply(QualityPreset.Low);
            }
        }

        /// <summary>UI ��ư�� ����</summary>
        public void ApplyLow() => Apply(QualityPreset.Low);
        public void ApplyMedium() => Apply(QualityPreset.Medium);
        public void ApplyHigh() => Apply(QualityPreset.High);

        /// <summary>Dropdown(int index)�� ����: 0=Low,1=Medium,2=High</summary>
        public void ApplyByIndex(int index)
        {
            Apply((QualityPreset)Mathf.Clamp(index, 0, 2));
        }

        public void Apply(QualityPreset preset)
        {
            if (urp == null)
            {
                Debug.LogWarning("[QualityPresetManager] URP Asset�� �����ϴ�. Project Settings > Graphics Ȯ��");
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
            // 2022.3 LTS�� URP Asset API
            urp.supportsHDR = enabled;
        }

        private void SetPostProcessing(bool enabled)
        {
            // ��� ī�޶� ��Ȯ��
            if (camData == null)
            {
                if (targetCamera == null) targetCamera = Camera.main;
                if (targetCamera != null)
                    camData = targetCamera.GetUniversalAdditionalCameraData();
            }

            if (camData != null)
                camData.renderPostProcessing = enabled;

            // �ʿ�� ���� ��� ī�޶� �����Ϸ��� �Ʒ� �ּ� ����
            // foreach (var cam in Camera.allCameras)
            // {
            //     var data = cam.GetUniversalAdditionalCameraData();
            //     if (data != null) data.renderPostProcessing = enabled;
            // }
        }

        // ������ ���� ������ ��ȸ
        public QualityPreset GetCurrentPreset() => lastApplied;
    }

    internal static class CameraExt
    {
        // UnityEngine.Rendering.Universal �� Ȯ�� �޼��带 ���� ȣ��(������ ����)
        public static UniversalAdditionalCameraData GetUniversalAdditionalCameraData(this Camera cam)
        {
            return cam != null ? cam.GetComponent<UniversalAdditionalCameraData>() : null;
        }
    }
}
