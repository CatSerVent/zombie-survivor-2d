#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildConfigurator
{
    // 공통 권장값 (근거: 컨텍스트의 성능 기본값과 모바일 권장 관행)
    private static void ApplyCommonPlayerSettings()
    {
        // vSync=0은 QualitySettings로, 60fps는 런타임 Boot에서 설정
        QualitySettings.vSyncCount = 0;
        PlayerSettings.gcIncremental = true;
        PlayerSettings.MTRendering = true;
        PlayerSettings.SplashScreen.show = false;

        // 플랫폼 비의존적/안전한 옵션만 남겼습니다.
        PlayerSettings.stripEngineCode = true;
    }

    [MenuItem("Tools/Build/Configure/Android")]
    public static void ConfigureAndroid()
    {
        ApplyCommonPlayerSettings();

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // 스크립팅 백엔드/ABI
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        // Min/Target API
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;   // 7.0+
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto; // <-- 교체 포인트

        // Graphics API: GLES3 (호환성 우선)
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] {
            UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3
        });

        // Stripping
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Medium);

        // App Bundle 권장
        EditorUserBuildSettings.buildAppBundle = true;

        Debug.Log("[BuildConfigurator] Android 설정 적용 완료");
    }

    [MenuItem("Tools/Build/Build/Android (AAB)")]
    public static void BuildAndroidAab()
    {
        ConfigureAndroid();

        string path = EditorUtility.SaveFilePanel("Save Android AAB", "Builds/Android", "ZombieSurvivor.aab", "aab");
        if (string.IsNullOrEmpty(path)) return;

        var scenes = GetEnabledScenes();
        var report = BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.CompressWithLz4HC);
        LogReport(report, path);
    }

    [MenuItem("Tools/Build/Configure/WebGL")]
    public static void ConfigureWebGL()
    {
        ApplyCommonPlayerSettings();

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);

        // WebGL Player Settings (근거: 용량/호환성 우선)
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
        PlayerSettings.WebGL.debugSymbols = false;
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        PlayerSettings.WebGL.memorySize = 256;     // 필요 시 384/512로 조정
        PlayerSettings.WebGL.threadsSupport = false;

        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, ManagedStrippingLevel.Medium);

        Debug.Log("[BuildConfigurator] WebGL 설정 적용 완료");
    }

    [MenuItem("Tools/Build/Build/WebGL")]
    public static void BuildWebGL()
    {
        ConfigureWebGL();

        string folder = EditorUtility.SaveFolderPanel("Select WebGL Build Folder", "Builds/WebGL", "WebGL");
        if (string.IsNullOrEmpty(folder)) return;

        var scenes = GetEnabledScenes();
        var report = BuildPipeline.BuildPlayer(scenes, Path.Combine(folder, "index.html"), BuildTarget.WebGL, BuildOptions.CompressWithLz4HC);
        LogReport(report, folder);
    }

    private static string[] GetEnabledScenes()
    {
        var tmp = EditorBuildSettings.scenes;
        var list = new System.Collections.Generic.List<string>();
        foreach (var s in tmp)
            if (s.enabled) list.Add(s.path);
        return list.ToArray();
    }

    private static void LogReport(BuildReport report, string path)
    {
        if (report.summary.result == BuildResult.Succeeded)
            Debug.Log($"[BuildConfigurator] Build Succeeded: {path} (size: {report.summary.totalSize / (1024f * 1024f):0.0} MB)");
        else
            Debug.LogError($"[BuildConfigurator] Build Failed: {report.summary.result}");
    }
}
#endif
