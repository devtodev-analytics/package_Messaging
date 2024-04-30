#if UNITY_IOS
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using UnityEngine;

namespace DevToDev
{
    public static class DTDPostProcessMessaging
    {
        private const string APP_TARGET_NAME = "Unity-iPhone";
        private const string UNITY_MESSAGING_NAME = "DTDMessagingUnity.xcframework";
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
                var frameworkAbsolutePath = Path.Combine("Plugins", "DevToDev", "Messaging", "IOS");
                frameworkAbsolutePath = Path.Combine(Application.dataPath, frameworkAbsolutePath, UNITY_MESSAGING_NAME);

                if (!Directory.Exists(frameworkAbsolutePath))
                {
                    frameworkAbsolutePath = Path.Combine("Packages", PACKAGE_NAME, "Plugins", "Messaging", "IOS");
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
            DirectoryCopy(frameworkPath, destinationFrameworkFilePath, true);
            // Add declaration to .xcodeproj.
            var fileInBuild =
                project.AddFile($"Frameworks/{UNITY_MESSAGING_NAME}", $"Frameworks/{UNITY_MESSAGING_NAME}");
            //project.AddFileToBuild(targetGuid, fileInBuild);
            project.AddFileToEmbedFrameworks(targetGuid, fileInBuild);
            var unityFrameworkLinkPhaseGuid = project.GetFrameworksBuildPhaseByTarget(targetGuid);
            project.AddFileToBuildSection(targetGuid, unityFrameworkLinkPhaseGuid, fileInBuild);
            project.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        }


        /// <summary>
        /// Recursively directory copy.
        /// </summary>
        /// <param name="sourceDirName">Source dit path.</param>
        /// <param name="destDirName">Destination dir path.</param>
        /// <param name="copySubDirs">Copy sub directories?</param>
        /// <param name="specificExtensions">
        /// If not empty will be copied only files with specific extensions.
        /// Example of extension: ".dll"
        /// </param>
        // Source: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs,
            params string[] specificExtensions)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: {sourceDirName}");
            }

            // Delete destination directory if exist.
            if (Directory.Exists(destDirName))
            {
                Directory.Delete(destDirName, true);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = null;
            if (specificExtensions != null && specificExtensions.Length > 0)
            {
                // Get the files of the specific extensions in the directory.
                files = dir.GetFiles().Where(x => specificExtensions.Contains(x.Extension)).ToArray();
            }
            else
            {
                // Get the all files in the directory.
                files = dir.GetFiles();
            }

            files = files.Where(x => x.Extension != ".meta").Where(x => x.Extension != ".DS_Store").ToArray();
            // Copy files to the new location
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }


            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs, specificExtensions);
                }
            }
        }
    }
}
#endif