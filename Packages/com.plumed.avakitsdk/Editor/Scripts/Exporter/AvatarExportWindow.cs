using System;
using System.Collections.Generic;
using UniGLTF;
using UnityEditor;
using UnityEngine;
using VRM;
using AvakitSDK.UI;

namespace AvakitSDK.Exporter
{
    public partial class AvatarExportWindow : EditorWindow
    {
        #region Variables

        private string _textLabel;
        private Vector2 _scrollPos;

        private VRMMetaObject _vrmMeta;
        private ExporterState _vrm;
        
        private bool _rootValid;
        private bool _componentValid;
        private bool _materialValid;

        private RectOffset _defaultRectOffset;
        private RectOffset _rectOffset;

        #endregion Variables

        #region Help Methods

        public static void Open()
        {
            var window = GetWindow<AvatarExportWindow>();
            window.titleContent = new GUIContent("Avakit Model Exporter");
            window.Show();
        }

        protected virtual void OnEnable()
        {
            Undo.willFlushUndoRecord += Repaint;
            Selection.selectionChanged += Repaint;

            _defaultRectOffset = new RectOffset(0, 0, 0, 0);
            _rectOffset = new RectOffset(8, 8, 8, 8);

            _vrm = new ExporterState();

            Initialize();

            _vrm.RootChanged += (root) =>
            {
                Repaint();
            };
            _vrm.Root = Selection.activeObject as GameObject;
        }

        private void Initialize()
        {
            _vrm.RootChanged += (root) =>
            {
                // Debug.Log("Root changed");
                // update meta
                if (root == null)
                {
                    _vrmMeta = null;
                }
                else
                {
                    var meta = root.GetComponent<VRMMeta>();
                    if (meta != null)
                    {
                        _vrmMeta = meta.Meta;
                    }
                    else
                    {
                        _vrmMeta = null;
                    }
                }
            };
        }

        #endregion Help Methods

        #region OnGUI

        private void OnGUI()
        {
            CustomGUI();
            BeginGUI();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
            {
                RootGUI();
                ComponentGUI();
                MaterialGUI();
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
            GUILayout.Space(8);
            _vrm.Root = (GameObject)EditorGUILayout.ObjectField("VRM Root", _vrm.Root, typeof(GameObject), true);
        }

        private void RootGUI()
        {
            var isRootExist = AvatarChecker.SetAvatar(_vrm.Root);
            if (!isRootExist)
            {
                _rootValid = false;
                return;
            }
            
            GUILayout.BeginVertical();
            {
                GUILayout.Label(Localization.Words.RootCheck);
                var validations = new List<Validation>();
                _rootValid = AvatarChecker.CheckRoot(ref validations);
                foreach (var validation in validations)
                {
                    ValidationDrawer.DrawGUI(validation);
                }
            }
            GUILayout.EndVertical();
        }

        private void ComponentGUI()
        {
            if (!_rootValid) return;
            
            GUILayout.BeginVertical();
            {
                GUILayout.Label(Localization.Words.ComponentCheck);
                InformationDrawer.DrawGUI(Localization.Words.ComponentNotice);
                var validations = new List<(Validation, Component)>();
                _componentValid = AvatarComponentChecker.CheckComponents(ref validations);
                foreach (var validation in validations)
                {
                    if (validation.Item2 != null)
                    {
                        var extended = new Action(() =>
                        {
                            GUILayout.BeginVertical(GUILayout.MinWidth(92));
                            {
                                GUILayout.Space(EditorGUIUtility.singleLineHeight + 3);
                                if (GUILayout.Button(Localization.Words.Delete))
                                {
                                    AvatarComponentChecker.RemoveInvalidComponent(validation.Item2);
                                }
                            }
                            GUILayout.EndVertical();
                        }); 
                        ValidationDrawer.DrawGUI(validation.Item1, extended);
                    }
                    else
                    {
                        ValidationDrawer.DrawGUI(validation.Item1);
                    }
                }

                if (!_componentValid)
                {
                    // Delete all code
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(Localization.Words.DeleteAll, GUILayout.MinWidth(100)))
                        {
                            AvatarComponentChecker.RemoveInvalidComponents(validations);
                        }
                        
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

        private void MaterialGUI()
        {
            if (!_rootValid) return;
            
            GUILayout.BeginVertical();
            {
                GUILayout.Label(Localization.Words.MaterialCheck);
                InformationDrawer.DrawGUI(Localization.Words.ShaderNotice);
                var validations = new List<(Validation, Material)>();
                _materialValid = AvatarMaterialChecker.CheckMaterials(ref validations);
                foreach (var validation in validations)
                {
                    if (validation.Item2 != null)
                    {
                        var extended = new Action(() =>
                        {
                            GUILayout.BeginVertical(GUILayout.MinWidth(92));
                            {
                                GUILayout.Space(EditorGUIUtility.singleLineHeight + 3);
                                if (GUILayout.Button(Localization.Words.Resolve))
                                {
                                    AvatarMaterialChecker.ChangeInvalidShaderToDefault(validation.Item2);
                                }
                            }
                            GUILayout.EndVertical();
                        }); 
                        ValidationDrawer.DrawGUI(validation.Item1, extended);
                    }
                    else
                    {
                        ValidationDrawer.DrawGUI(validation.Item1);
                    }
                }


                if (!_materialValid)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(Localization.Words.ResolveAll, GUILayout.MinWidth(100)))
                        {
                            AvatarMaterialChecker.RemoveInvalidMaterials(validations);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
        
        private void EndGUI()
        {
            var canExport = _rootValid && _componentValid && _materialValid;
            
            GUI.skin.button.margin = _rectOffset;
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUI.enabled = canExport;
                
                if (GUILayout.Button(Localization.Words.Export, GUILayout.MinWidth(108)))
                {
                    var success = AvatarExporter.ExportAvatarToFile(_vrm.Root);
                    if (success)
                    {
                        Close();
                        GUIUtility.ExitGUI();
                    }
                }
                GUI.enabled = true;

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUILayout.Space(8);
        }

        #endregion
    }
}
