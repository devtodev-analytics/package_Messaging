#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace DevToDev
{
    public static class DTDPostProcessMessaging
    {
        private const string DEVICE = "ios-arm64_armv7";
        private const string SIMULATOR = "ios-arm64_i386_x86_64-simulator";
        const string APP_TARGET_NAME = "Unity-iPhone";
        private const string UNITY_MESSAGING_NAME = "DTDMessagingUnity.framework";
        
        [PostProcessBuild(97)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.iOS)
            {
                var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var project = new PBXProject();
                project.ReadFromString(File.ReadAllText(projectPath));
#if UNITY_2018
                var name = PBXProject.GetUnityTargetName();
                var targetGuid = project.TargetGuidByName(name);
#else
                var targetGuid = project.GetUnityFrameworkTargetGuid();
#endif
                project.AddFrameworkToProject(targetGuid, "UserNotifications.framework", true);
                AddMessagingFramework(pathToBuiltProject, project, targetGuid,
                    PlayerSettings.iOS.sdkVersion == iOSSdkVersion.DeviceSDK
                        ? Path.Combine("Plugins", "DevToDev", "Messaging", "IOS", DEVICE)
                        : Path.Combine("Plugins", "DevToDev", "Messaging", "IOS", SIMULATOR));
                project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
                File.WriteAllText(projectPath, project.WriteToString());
            }
        }

        private static void AddMessagingFramework(string projectPath, PBXProject project, string targetGuid,
            string frameworkFolderName)
        {
            var destinationFrameworkFilePath = Path.Combine(projectPath, "Frameworks", UNITY_MESSAGING_NAME);
            var editorFrameworkFilePath = Path.Combine(Application.dataPath,
                frameworkFolderName, UNITY_MESSAGING_NAME);
            // Copy file
            CopyAndReplaceDirectory(editorFrameworkFilePath, destinationFrameworkFilePath);
            // Add declaration to .xcodeproj.
            var fileInBuild =
                project.AddFile($"Frameworks/{UNITY_MESSAGING_NAME}", $"Frameworks/{UNITY_MESSAGING_NAME}");
            project.AddFileToBuild(targetGuid, fileInBuild);
            project.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        }

        private static void CopyAndReplaceDirectory(string srcPath, string dstPath)
        {
            if (Directory.Exists(dstPath))
                Directory.Delete(dstPath);
            if (File.Exists(dstPath))
                File.Delete(dstPath);

            Directory.CreateDirectory(dstPath);

            foreach (var file in Directory.GetFiles(srcPath))
                File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

            foreach (var dir in Directory.GetDirectories(srcPath))
                CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
        }
    }
}
#endif