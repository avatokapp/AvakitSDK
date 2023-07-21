using System;
using UnityEditor;
using UnityEngine;

namespace AvakitSDK.Installer
{
    public class AvatarInstallerWindow : EditorWindow
    {
        #region Variables

        private Vector2 _scrollPos;
        private Vector2 _packageScrollPos;
        private Vector2 _shaderScrollPos;
        
#if UNITY_2021_3_1 || UNITY_2021_3_2 || UNITY_2021_3_3 || UNITY_2021_3_4 || UNITY_2021_3_5 || UNITY_2021_3_6 || UNITY_2021_3_7 || UNITY_2021_3_8
        private const bool UnitySupported = true;
#else
        private const bool UnitySupported = false;
#endif

        private RectOffset _defaultRectOffset;
        private RectOffset _rectOffset;

        private const string AvaKitDiscordURL = "https://discord.gg/e8qGrZA3CT";

        #endregion Variables

        #region Help Methods

        public static void Open()
        {
            var window = GetWindow<AvatarInstallerWindow>();
            window.titleContent = new GUIContent("Avakit SDK Installer");
            window.Show();
        }

        protected virtual void OnEnable()
        {
            // Undo.willFlushUndoRecord += Repaint;
            // Selection.selectionChanged += Repaint;
            AvakitSDKInstaller.OnAllPackagesInstalled += Repaint;

            _defaultRectOffset = new RectOffset(0, 0, 0, 0);
            _rectOffset = new RectOffset(8, 8, 8, 8);
        }

        #endregion Help Methods

        #region OnGUI

        private void OnGUI()
        {
            CustomGUI();
            BeginGUI();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
            {
                if (UnitySupported)
                {
                    PackageGUI();
                    ShaderGUI();
                }
                else
                {
                    DrawMessageGUI(Localization.Words.UnityVersionNotice, isError: true);
                }
            }
            EditorGUILayout.EndScrollView();

            DefaultGUI();
            EndGUI();
        }

        private void CustomGUI()
        {
            GUI.skin.scrollView.margin = _rectOffset;
        }

        private void DefaultGUI()
        {
            GUI.skin.scrollView.margin = _defaultRectOffset;
        }

        private void BeginGUI()
        {
            Localization.Lang = (Localization.Languages)EditorGUILayout.EnumPopup(Localization.Words.Language, Localization.Lang);
        }

        private void PackageGUI()
        {   
            GUILayout.BeginVertical();
            {
                GUILayout.Label(Localization.Words.PackageNotice);
                DrawStringListGUI(ref _packageScrollPos, RequiredPackageNames);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUI.enabled = !(AvakitSDKInstaller.AllPackagesInstalled || AvakitSDKInstaller.IsInstalling);
                if (GUILayout.Button(Localization.Words.InstallAll, GUILayout.MinWidth(100)))
                {
                    AvakitSDKInstaller.AddPackageWithIndex();
                }
                GUI.enabled = true;
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void ShaderGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label(Localization.Words.ShaderNotice);
                DrawStringListGUI(ref _shaderScrollPos, SupportedShaderNames);
            }
            GUILayout.EndVertical();
        }
        
        private void EndGUI()
        {   
            GUI.skin.button.margin = _rectOffset;
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(Localization.Words.Discord, GUILayout.MinWidth(108)))
                {
                    Application.OpenURL(AvaKitDiscordURL);
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(Localization.Words.Close, GUILayout.MinWidth(108)))
                {
                    Close();
                    GUIUtility.ExitGUI();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUILayout.Space(8);
        }

        #endregion OnGUI

        #region Help Methods

        private static void DrawMessageGUI(string message, bool isInfo = false, bool isError = false)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (isInfo)
                    {
                        EditorGUILayout.LabelField(Info, GUILayout.Width(30), GUILayout.Height(30));
                    }
                    if (isError)
                    {
                        EditorGUILayout.LabelField(Error, GUILayout.Width(30), GUILayout.Height(30));
                    }
                    
                    using (new GUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label(message, EditorStyles.wordWrappedLabel);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.Space(8);
                    }
                }
            }
        }

        private static void DrawStringListGUI(ref Vector2 scrollPos, string[] list)
        {
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, GUILayout.MinHeight(64), GUILayout.MaxHeight(78));
                {
                    foreach (var entry in list)
                    {
                        GUILayout.Label(entry, GUI.skin.label);
                    }
                }
                GUILayout.EndScrollView();
            }
        }

        #endregion Help Methods
        
        #region Validation Extensions
        
        static GUIContent _info;
        public static GUIContent Info
        {
            get
            {
                if (_info == null)
                {
                    _info = EditorGUIUtility.IconContent("console.infoicon");
                }
                return _info;
            }
        }
        static GUIContent _error;
        public static GUIContent Error
        {
            get
            {
                if (_error == null)
                {
                    _error = EditorGUIUtility.IconContent("console.erroricon");
                }
                return _error;
            }
        }

        #endregion Validation Extensions
        
        #region SupportedShader

        private static readonly string[] RequiredPackageNames =
        {
            "UniVRM",
            "UniGLTF",
            "VRMShaders",
            "lilToon"
        };

        private static readonly string[] SupportedShaderNames =
        {
            // UniGLTF & VRM default shader
            "UniGLTF - UniUnlit",
            "VRM - MToon",
            "lilToon",
            "UnlitWF",
            // "VRM10/MToon10",
            // Unity default shader
            "Unity - Standard",
            "Unity - Unlit",
            "Unity - Legacy Shaders",
            "Unity - Universal Render Pipline Shaders"
        };

        #endregion SupportedShader
    }
}
