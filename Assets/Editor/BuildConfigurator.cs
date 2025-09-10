#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildConfigurator
{
    // ���� ���尪 (�ٰ�: ���ؽ�Ʈ�� ���� �⺻���� ����� ���� ����)
    private static void ApplyCommonPlayerSettings()
    {
        // vSync=0�� QualitySettings��, 60fps�� ��Ÿ�� Boot���� ����
        QualitySettings.vSyncCount = 0;
        PlayerSettings.gcIncremental = true;
        PlayerSettings.MTRendering = true;
        PlayerSettings.SplashScreen.show = false;

        // �÷��� ��������/������ �ɼǸ� ������ϴ�.
        PlayerSettings.stripEngineCode = true;
    }

    [MenuItem("Tools/Build/Configure/Android")]
    public static void ConfigureAndroid()
    {
        ApplyCommonPlayerSettings();

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // ��ũ���� �鿣��/ABI
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        // Min/Target API
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;   // 7.0+
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto; // <-- ��ü ����Ʈ

        // Graphics API: GLES3 (ȣȯ�� �켱)
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] {
            UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3
        });

        // Stripping
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Medium);

        // App Bundle ����
        EditorUserBuildSettings.buildAppBundle = true;

        Debug.Log("[BuildConfigurator] Android ���� ���� �Ϸ�");
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

        // WebGL Player Settings (�ٰ�: �뷮/ȣȯ�� �켱)
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.decompressionFallback = true;
        PlayerSettings.WebGL.dataCaching = true;
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
        PlayerSettings.WebGL.debugSymbols = false;
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        PlayerSettings.WebGL.memorySize = 256;     // �ʿ� �� 384/512�� ����
        PlayerSettings.WebGL.threadsSupport = false;

        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, ManagedStrippingLevel.Medium);

        Debug.Log("[BuildConfigurator] WebGL ���� ���� �Ϸ�");
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
