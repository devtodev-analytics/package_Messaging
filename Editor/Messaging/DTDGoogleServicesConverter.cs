using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class DTDGoogleServicesConverter : IPreprocessBuildWithReport
{
    private const string GoogleServicesAssetName = "google-services";
    private const string GoogleServicesFullname = GoogleServicesAssetName + ".json";

    private const string Prefix = "DevToDev : ";
    private const string OutputFile = "DevToDev.xml";
    private const string OutputDirectory = "Assets/Resources/DevToDev";

    public const string SenderIdFieldName = "sender_id";
    public const string ProjectIdFieldName = "project_id";
    public const string MobilesdkAppIdFieldName = "mobilesdk_app_id";
    public const string CurrentKeyFieldName = "current_key";


    private string GetGoogleJsonPath()
    {
        var googleServicesFiles = AssetDatabase.FindAssets(GoogleServicesAssetName).Select(AssetDatabase.GUIDToAssetPath).Where(assetPath => Path.GetFileName(assetPath) == GoogleServicesFullname).ToList();

        if (googleServicesFiles.Count <= 0)
        {
            return null;
        }

        if (googleServicesFiles.Count > 1)
        {
            Debug.LogWarning(
                $"{Prefix}{googleServicesFiles.Count} {GoogleServicesAssetName} file found, using first one.");
        }

        return googleServicesFiles[0];

    }

    private void SavePushDataToResources()
    {
        var googleServicesPath = GetGoogleJsonPath();
        if (string.IsNullOrEmpty(googleServicesPath))
        {
            return;
        }

        var projectDir = Path.Combine(Application.dataPath, "..");
        var inputPath = Path.Combine(projectDir, googleServicesPath);
        var json = File.ReadAllText(inputPath);
        var googleServices = JsonUtility.FromJson<GoogleServicesJson>(json);

        var projectId = googleServices.project_info?.project_id;
        var sdk = googleServices.client[0]?.client_info?.mobilesdk_app_id;
        var currentKey = googleServices.client[0]?.api_key[0]?.current_key;
        var senderId = googleServices.project_info?.project_number;

        CreateDirectories();

        const string outputPath = OutputDirectory + "/" + OutputFile;
        var outputFile = new XDocument(new XElement("push_data",
                new XElement(SenderIdFieldName, senderId),
                new XElement(ProjectIdFieldName, projectId),
                new XElement(MobilesdkAppIdFieldName, sdk),
                new XElement(CurrentKeyFieldName, currentKey)
            )
        );
        outputFile.Save(outputPath);
        AssetDatabase.Refresh();
    }

    private void CreateDirectories()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            try
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            catch (Exception e)
            {
                Debug.LogError($"{Prefix} failed to create resources folder\n{e}");
            }
        }

        if (!AssetDatabase.IsValidFolder(OutputDirectory))
        {
            try
            {
                AssetDatabase.CreateFolder("Assets/Resources", "DevToDev");
            }
            catch (Exception e)
            {
                Debug.LogError($"{Prefix} failed to create DevToDev folder in resources folder.\n{e}");
            }
        }
    }

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
#if UNITY_ANDROID
        SavePushDataToResources();
#endif
    }

    #region GoogleServicesModel

    [Serializable]
    public class ProjectInfo
    {
        public string project_number;
        public string firebase_url;
        public string project_id;
        public string storage_bucket;
    }

    [Serializable]
    public class AndroidClientInfo
    {
        public string package_name;
    }

    [Serializable]
    public class ClientInfo
    {
        public string mobilesdk_app_id;
        public AndroidClientInfo android_client_info;
    }

    [Serializable]
    public class OauthClient
    {
        public string client_id;
        public int client_type;
    }

    [Serializable]
    public class ApiKey
    {
        public string current_key;
    }

    [Serializable]
    public class IosInfo
    {
        public string bundle_id;
    }

    [Serializable]
    public class OtherPlatformOauthClient
    {
        public string client_id;
        public int client_type;
        public IosInfo ios_info;
    }

    [Serializable]
    public class AppinviteService
    {
        public List<OtherPlatformOauthClient> other_platform_oauth_client;
    }

    [Serializable]
    public class Services
    {
        public AppinviteService appinvite_service;
    }

    [Serializable]
    public class Client
    {
        public ClientInfo client_info;
        public List<OauthClient> oauth_client;
        public List<ApiKey> api_key;
        public Services services;
    }

    [Serializable]
    public class GoogleServicesJson
    {
        public ProjectInfo project_info;
        public List<Client> client;
        public string configuration_version;
    }

    #endregion
}
