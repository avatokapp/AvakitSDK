using System;
using UnityEditor;
using UnityEngine;

namespace AvakitSDK.Exporter
{
    public partial class AvatarExportWindow
    {
        private class ExporterState : IDisposable
        {
            #region Variables

            (GameObject GameObject, bool IsPrefab) _root;
            
            public event Action<GameObject> RootChanged;
            private void OnRootChanged()
            {
                var tmp = RootChanged;
                if (tmp == null) return;
                tmp(_root.GameObject);
            }
            
            public GameObject Root
            {
                get { return _root.GameObject; }
                set
                {
                    string assetPath = default;
                    var isPrefab = false;
                    if (value != null && AssetDatabase.IsMainAsset(value))
                    {
                        assetPath = AssetDatabase.GetAssetPath(value);
                        try
                        {
                            var prefab = PrefabUtility.LoadPrefabContents(assetPath);
                            value = prefab;
                            isPrefab = true;
                        }
                        catch (ArgumentException)
                        {
                            // Debug.LogWarning(ex);
                        }
                    }
                    if (_root.GameObject == value)
                    {
                        return;
                    }
                    if (_root.IsPrefab)
                    {
                        PrefabUtility.UnloadPrefabContents(_root.GameObject);
                    }
                    _root = (value, isPrefab);
                    // m_requireValidation = true;
                    OnRootChanged();
                }
            }

            #endregion

            public void Dispose()
            {
                Root = null;
                RootChanged = null;
            }
        }
    }
}