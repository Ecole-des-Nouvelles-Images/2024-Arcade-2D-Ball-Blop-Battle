using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

    public static class BuildRunner
    {
        private static string OutputDir => Path.Combine("Builds", "Windows", $"{Application.productName}_v{Application.version}");
        private static string VersionVariable => Application.version;
        
        /// <summary>Le commit livré, ajouté à la version pour que les journaux se rattachent au dépôt.</summary>
        private const string ShaVariable = "PROJECT_SHA";

        [MenuItem("Tools/Build and Publish on itchio")]
        public static void BuildAndPublish()
        {
            if (!BuildPlayerNow())
                return;

            string root = Directory.GetCurrentDirectory();
            string script = Path.Combine(root, "Tools", "upload-itchio.ps1");

            if (!File.Exists(script))
            {
                Debug.LogError($"[{nameof(BuildRunner)}] Script d'envoi introuvable : {script}");
                return;
            }
            
            string normalizedPath = Path.GetFullPath(OutputDir);

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -NoProfile -ExecutionPolicy Bypass -File \"{script}\" -SkipBuild -BuildPath \"{normalizedPath}\" -Version \"{VersionVariable}\"",
                WorkingDirectory = root,
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(psi);
            Debug.Log($"[{nameof(BuildRunner)}] Envoi lancé dans une console séparée.");
        }

        [MenuItem("Tools/Build")]
        public static void BuildWindows()
        {
            if (BuildPlayerNow() && Application.isBatchMode)
                EditorApplication.Exit(0);
        }

        /// <summary>La build elle-même. Rend `false` si elle a échoué — l'appelant décide de la suite.</summary>
        private static bool BuildPlayerNow()
        {
            string previousVersion = PlayerSettings.bundleVersion;
            PlayerSettings.bundleVersion = VersionVariable;

            string[] scenes = EditorBuildSettings.scenes
                                                 .Where(s => s.enabled && !string.IsNullOrEmpty(s.path))
                                                 .Select(s => s.path)
                                                 .ToArray();

            if (scenes.Length == 0)
            {
                Fail("Aucune scène activée dans les Build Settings : il n'y a rien à construire.");
                return false;
            }

            string output = Path.Combine(Directory.GetCurrentDirectory(), OutputDir);

            if (Directory.Exists(output))
                Directory.Delete(output, true);
            Directory.CreateDirectory(output);

            BuildPlayerOptions options = new()
            {
                scenes = scenes,
                locationPathName = Path.Combine(output, $"{Application.productName}.exe"),
                target = BuildTarget.StandaloneWindows64,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.None,
            };

            BuildReport report;
            string built = PlayerSettings.bundleVersion;
            try
            {
                report = BuildPipeline.BuildPlayer(options);
            }
            finally
            {
                PlayerSettings.bundleVersion = previousVersion;
                AssetDatabase.SaveAssets();
            }

            BuildSummary summary = report.summary;

            if (summary.result != BuildResult.Succeeded)
            {
                Fail($"Build {summary.result} — {summary.totalErrors} erreur(s).");
                return false;
            }

            Debug.Log($"[{nameof(BuildRunner)}] Build OK : {summary.totalSize / (1024 * 1024)} Mo, "
                      + $"version {built}, dans {OutputDir}");

            return true;
        }

        private static void Fail(string message)
        {
            Debug.LogError($"[{nameof(BuildRunner)}] {message}");

            if (Application.isBatchMode)
                EditorApplication.Exit(1);
        }
    }