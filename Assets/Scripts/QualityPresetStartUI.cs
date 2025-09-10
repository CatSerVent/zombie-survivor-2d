using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 게임 시작 시 품질 프리셋 선택 팝업을 보여줍니다.
    /// - Low/Medium/High 버튼으로 QualityPresetManager에 적용
    /// - "기억하기" 체크 시 다음 실행부터 자동 적용하고 팝업 생략
    /// - 팝업이 떠 있는 동안 일시정지(Time.timeScale = 0)
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class QualityPresetStartUI : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("프리셋을 실제로 적용하는 매니저 (씬에 1개 존재)")]
        public Game.Systems.Settings.QualityPresetManager presetManager;

        [Tooltip("팝업 루트 패널(비/활성 제어 대상)")]
        public GameObject popupRoot;

        [Tooltip("선택 저장 토글(없으면 null)")]
        public Toggle rememberToggle;

        [Header("Behavior")]
        [Tooltip("저장된 프리셋이 있으면 팝업을 자동 생략합니다.")]
        public bool skipWhenSaved = true;

        [Tooltip("PlayerPrefs 키 (QualityPresetManager와 동일하게 두세요).")]
        public string playerPrefsKey = "QualityPreset";

        [Tooltip("팝업 표시 시 게임을 일시정지합니다.")]
        public bool pauseWhileOpen = true;

        [Header("Debug/Control")]
        [Tooltip("진단 로그 출력")]
        public bool logDebug = true;

#if UNITY_EDITOR
        [Tooltip("에디터에서 Play할 때는 항상 팝업을 띄웁니다(저장 스킵 무시).")]
        public bool forceShowOnEditorPlay = true;
#endif

        float _prevTimeScale = 1f;

        void Awake()
        {
            if (presetManager == null)
                presetManager = FindObjectOfType<Game.Systems.Settings.QualityPresetManager>(includeInactive: true);

            if (popupRoot == null)
                Debug.LogWarning("[QualityPresetStartUI] popupRoot가 비어 있습니다. Panel_QualityPopup를 연결하세요.");

            // 디버그: 현재 저장 상태 로그
            if (logDebug)
                Debug.Log($"[QualityPresetStartUI] HasKey={PlayerPrefs.HasKey(playerPrefsKey)} val={PlayerPrefs.GetInt(playerPrefsKey, -1)}");

#if UNITY_EDITOR
            // 에디터에서는 강제로 항상 팝업 표시 (원인 파악/테스트 편의)
            if (forceShowOnEditorPlay)
            {
                SafeOpenPopup();
                return;
            }
#endif

            // 저장된 프리셋이 있고 스킵 옵션이 켜져 있으면 자동 적용 & 팝업 닫기
            if (skipWhenSaved && PlayerPrefs.HasKey(playerPrefsKey))
            {
                int idx = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey, 0), 0, 2);
                if (logDebug) Debug.Log($"[QualityPresetStartUI] Auto-apply saved preset index={idx} -> ClosePopup()");
                ApplyPreset((Game.Systems.Settings.QualityPreset)idx, save: true, closePopup: true);
                return;
            }

            // 기본: 팝업을 연다
            SafeOpenPopup();
        }

        void SafeOpenPopup()
        {
            if (popupRoot != null && !popupRoot.activeSelf)
                popupRoot.SetActive(true);

            if (pauseWhileOpen)
            {
                _prevTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
        }

        void ClosePopup()
        {
            if (popupRoot != null && popupRoot.activeSelf)
                popupRoot.SetActive(false);

            if (pauseWhileOpen)
                Time.timeScale = _prevTimeScale;
        }

        /// <summary>UI 버튼용 — Low</summary>
        public void OnClickLow()
        {
            ApplyPreset(Game.Systems.Settings.QualityPreset.Low, ShouldRemember(), closePopup: true);
        }

        /// <summary>UI 버튼용 — Medium</summary>
        public void OnClickMedium()
        {
            ApplyPreset(Game.Systems.Settings.QualityPreset.Medium, ShouldRemember(), closePopup: true);
        }

        /// <summary>UI 버튼용 — High</summary>
        public void OnClickHigh()
        {
            ApplyPreset(Game.Systems.Settings.QualityPreset.High, ShouldRemember(), closePopup: true);
        }

        /// <summary>내부 공통 처리</summary>
        void ApplyPreset(Game.Systems.Settings.QualityPreset preset, bool save, bool closePopup)
        {
            if (presetManager != null)
            {
                presetManager.Apply(preset);

                if (save)
                {
                    PlayerPrefs.SetInt(playerPrefsKey, (int)preset);
                    PlayerPrefs.Save();
                    if (logDebug) Debug.Log($"[QualityPresetStartUI] Saved preset index={(int)preset}");
                }
            }
            else
            {
                Debug.LogWarning("[QualityPresetStartUI] presetManager가 없습니다. 씬에 QualityPresetManager를 배치하고 연결하세요.");
            }

            if (closePopup)
                ClosePopup();
        }

        bool ShouldRemember()
        {
            // 토글이 없으면 저장하지 않음(원래 의도 유지). 항상 저장하려면 true로 바꾸세요.
            return rememberToggle != null && rememberToggle.isOn;
        }

        /// <summary>디버그용: 저장된 프리셋 키 삭제 후 즉시 팝업 강제 오픈</summary>
        public void DebugClearAndOpen()
        {
            PlayerPrefs.DeleteKey(playerPrefsKey);
            PlayerPrefs.Save();
            if (logDebug) Debug.Log("[QualityPresetStartUI] Cleared saved preset key and reopening popup");
            SafeOpenPopup();
        }
    }
}
