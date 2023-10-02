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
        private const string APP_TARGET_NAME = "Unity-iPhone";
        private const string UNITY_MESSAGING_NAME = "DTDMessagingUnity.framework";
        private const string PACKAGE_NAME = "com.devtodev.sdk.messaging";

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
                var frameworkAbsolutePath = PlayerSettings.iOS.sdkVersion == iOSSdkVersion.DeviceSDK
                    ? Path.Combine("Plugins", "DevToDev", "Messaging", "IOS", DEVICE)
                    : Path.Combine("Plugins", "DevToDev", "Messaging", "IOS", SIMULATOR);
                frameworkAbsolutePath = Path.Combine(Application.dataPath, frameworkAbsolutePath, UNITY_MESSAGING_NAME);

                if (!Directory.Exists(frameworkAbsolutePath))
                {
                    frameworkAbsolutePath = PlayerSettings.iOS.sdkVersion == iOSSdkVersion.DeviceSDK
                        ? Path.Combine("Packages", PACKAGE_NAME, "Plugins", "Messaging", "IOS", DEVICE)
                        : Path.Combine("Packages", PACKAGE_NAME, "Plugins", "Messaging", "IOS", SIMULATOR);
                    frameworkAbsolutePath = Path.GetFullPath(Path.Combine(frameworkAbsolutePath, UNITY_MESSAGING_NAME));
                }

                AddMessagingFramework(pathToBuiltProject, project, targetGuid,
                    frameworkAbsolutePath);
                project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
                File.WriteAllText(projectPath, project.WriteToString());
            }
        }

        private static void AddMessagingFramework(string projectPath, PBXProject project, string targetGuid,
            string frameworkPath)
        {
            var destinationFrameworkFilePath = Path.Combine(projectPath, "Frameworks", UNITY_MESSAGING_NAME);
            // Copy file
            CopyAndReplaceDirectory(frameworkPath, destinationFrameworkFilePath);
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