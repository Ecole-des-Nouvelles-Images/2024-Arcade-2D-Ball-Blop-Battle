// À placer dans Assets/Editor/BuildScript.cs

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

    public static class BuildScript
    {
        // Appelé via : Unity.exe -batchmode -buildTarget <cible> -executeMethod BuildScript.Build -customBuildPath <dossier>
        public static void Build()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            string outDir = GetArg("-customBuildPath") ?? "Build";
            string name = PlayerSettings.productName;

            string path = target switch
            {
                BuildTarget.StandaloneWindows64 => $"{outDir}/{name}.exe",
                BuildTarget.StandaloneLinux64   => $"{outDir}/{name}.x86_64",
                BuildTarget.StandaloneOSX       => $"{outDir}/{name}.app",
                BuildTarget.Android             => $"{outDir}/{name}.{(EditorUserBuildSettings.buildAppBundle ? "aab" : "apk")}",
                _                               => outDir // WebGL et autres : un dossier
            };

            // Android : mots de passe du keystore lus depuis les secrets GitHub (jamais dans le repo)
            if (target == BuildTarget.Android)
            {
                string ksPass = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_PASS");
                string aliasPass = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_PASS");
                if (!string.IsNullOrEmpty(ksPass)) PlayerSettings.Android.keystorePass = ksPass;
                if (!string.IsNullOrEmpty(aliasPass)) PlayerSettings.Android.keyaliasPass = aliasPass;
            }

            var options = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
                locationPathName = path,
                target = target,
                targetGroup = BuildPipeline.GetBuildTargetGroup(target),
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;
            Debug.Log($"[BuildScript] {summary.result} | {summary.totalErrors} erreur(s) | {summary.totalSize / (1024 * 1024)} Mo | {summary.totalTime}");

            EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
        }

        private static string GetArg(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }
    }
