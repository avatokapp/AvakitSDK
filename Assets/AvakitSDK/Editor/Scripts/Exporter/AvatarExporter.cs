using System.IO;
using UnityEngine;
using UnityEditor;
using VRM;
using AvakitSDK.Avatar;

namespace AvakitSDK.Exporter
{
    public static class AvatarExporter
    {
        #region Variables

        private static GameObject SelectedAvatar;

        #endregion Variables

        #region Path

        private static string _prefabPath;
        private static string _saveFilePath;
        private static string _saveFileName;

        private static void SetSaveFilePath() => _saveFilePath = EditorUtility.SaveFilePanel("Avatar Exporter", ".", SelectedAvatar.name, AvakitAvatar.AssetExtension);
        private static bool SaveFilePathError
        {
            get
            {
                if (!string.IsNullOrEmpty(_saveFilePath))
                {
                    Debug.Log("[Export Avatar] Save File Path : " + _saveFilePath);
                    return false;
                }
                
                Debug.LogError("[Export Avatar] Export Path Null Or Empty");
                return true;

            }
        }
        private static void SetFileName() => _saveFileName = Path.GetFileName(_saveFilePath);
        
        #endregion Path
        
        #region Help Method
        
        [MenuItem("Avakit/Export Avatar")]
        public static void ExportAvatar()
        {
            Debug.Log("[Export Avatar] Start");
            AvatarExportWindow.Open();
        }

        public static bool ExportAvatarToFile(GameObject avatar = null)
        {
            _prefabPath = AvakitAvatar.PrefabPath;
            
            SelectedAvatar = avatar;
            ChangeThumbnailImportSetting(avatar.GetComponent<VRMMeta>().Meta.Thumbnail);

            SetSaveFilePath();
            if (SaveFilePathError) return false;

            SetFileName();
            SaveAvatar();
            return true;
        }

        private static void ChangeThumbnailImportSetting(Texture2D thumbnail)
        {
            if (thumbnail == null) return;
            var path = AssetDatabase.GetAssetPath(thumbnail);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }

        private static void SaveAvatar()
        {
            try
            {
                DeleteTempPrefab();

                TrySaveAvatarToPrefab(out var failedSaveToPrefab);
                if (failedSaveToPrefab) return;

                SaveToAssetBundle();
                EditorUtility.DisplayDialog(Localization.Words.Export, "Export complete!", "OK");
            }
            finally
            {
                DeleteTempPrefab();
            }
        }
        
        private static void DeleteTempPrefab()
        {
            AssetDatabase.DeleteAsset(_prefabPath);
            DeleteFile(_prefabPath);
        }

        private static void TrySaveAvatarToPrefab(out bool saveFailed)
        {
            PrefabUtility.SaveAsPrefabAsset(SelectedAvatar, _prefabPath, out var saveState);
            saveFailed = !saveState;
            
            if (saveFailed) Debug.LogError("[Export Avatar] Save Avatar To Prefab Failed");
        }

        private static void SaveToAssetBundle()
        {
            var bundleBuild = new AssetBundleBuild
            {
                assetBundleName = _saveFileName,
                assetNames = new [] { _prefabPath },
                addressableNames = new [] { AvakitAvatar.AssetName }
            };
#if UNITY_EDITOR_WIN
            BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new [] { bundleBuild }, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
#elif UNITY_EDITOR_OSX
            BuildPipeline.BuildAssetBundles(Application.temporaryCachePath, new [] { bundleBuild }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
#endif
            DeleteFile(_saveFilePath);
            File.Move(Application.temporaryCachePath + "/" + _saveFileName, _saveFilePath);
        }

        private static void DeleteFile(string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }
        
        #endregion Help Method

    }
}

