using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEngine;
using System.IO;

namespace AvakitSDK.Installer
{
    public static class AvakitSDKInstaller
    {
        private static (string Id, string Address, bool IsInstalled)[] _packages = 
        {
            ("jp.lilxyzw.liltoon", "https://github.com/lilxyzw/lilToon.git?path=Assets/lilToon#1.4.0", false),
            ("com.vrmc.vrmshaders", "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders#v0.112.0", false),
            ("com.vrmc.gltf", "https://github.com/vrm-c/UniVRM.git?path=/Assets/UniGLTF#v0.112.0", false),
            ("com.vrmc.univrm", "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM#v0.112.0", false),
            ("com.vrmc.vrm", "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM10#v0.112.0", false),
            ("com.plumed.avakitsdk", "https://github.com/avatokapp/AvakitSDK.git?path=Packages/com.plumed.avakitsdk#0.0.2", false)
        };

        private static AddRequest _request;
        private static int _index = 0;

        public static bool AllPackagesInstalled = false;
        public static bool IsInstalling = false;
        public static System.Action OnAllPackagesInstalled;

        [MenuItem("Avakit/Add Packages")]
        static void Add()
        {
            if (!IsInstalling)
                _index = -1;
            // Add a package to the project
            CheckPackagesInstalled();
            AvatarInstallerWindow.Open();
        }

        public static void AddPackageWithIndex()
        {
            IsInstalling = true;
            _index++;
            if (_index >= _packages.Length)
            {
                CheckPackagesInstalled();
                OnAllPackagesInstalled?.Invoke();
                if (AllPackagesInstalled)
                    Debug.Log("All packages installed successfully!");
                IsInstalling = false;
                return;
            }
            
            if (_packages[_index].IsInstalled)
            {
                AddPackageWithIndex();
                return;
            }
            _request = Client.Add(_packages[_index].Id + "@" + _packages[_index].Address);
            EditorApplication.update += Progress;
        }

        private static void Progress()
        {
            if (_request.IsCompleted)
            {
                if (_request.Status == StatusCode.Success)
                {
                    _packages[_index].IsInstalled = true;
                    Debug.Log("Installed: " + _request.Result.packageId);
                }
                else if (_request.Status >= StatusCode.Failure)
                {
                    Debug.Log(_request.Error.message);
                }
                EditorApplication.update -= Progress;
                
                AddPackageWithIndex();
            }
        }
        
        private static void CheckPackagesInstalled()
        {
            if ( !File.Exists("Packages/manifest.json") )
                return;
 
            var jsonText = File.ReadAllText("Packages/manifest.json");
            
            var count = _packages.Length;
            var allPackagesInstalled = true;
            var isInstalled = false;
            for (var i = 0; i < count; i++)
            {
                isInstalled = jsonText.Contains( _packages[i].Id + "\"" );
                _packages[i].IsInstalled = isInstalled;
                allPackagesInstalled = allPackagesInstalled && isInstalled;
            }

            AllPackagesInstalled = allPackagesInstalled;
        }
    }
}