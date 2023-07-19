using System;
using UniGLTF;
using UnityEditor;
using UnityEngine;

namespace AvakitSDK.UI
{
    public class ValidationDrawer
    {
        
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

        static GUIContent _warn;
        public static GUIContent Warn
        {
            get
            {
                if (_warn == null)
                {
                    _warn = EditorGUIUtility.IconContent("console.warnicon");
                }
                return _warn;
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
        
        #region DrawGUI
        
        public static void DrawGUI(ErrorLevels levels, string msg)
        {
            switch (levels)
            {
                case ErrorLevels.Info:
                    DrawGUI(Validation.Info(msg));
                    break;
                case ErrorLevels.Warning:
                    DrawGUI(Validation.Warning(msg));
                    break;
                case ErrorLevels.Error:
                    DrawGUI(Validation.Error(msg));
                    break;
                case ErrorLevels.Critical:
                    DrawGUI(Validation.Critical(msg));
                    break;
            }
        }
        
        public static void DrawGUI(Validation self, Action extended = null)
        {
            if (string.IsNullOrEmpty(self.Message))
            {
                return;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.HorizontalScope())
                {
                    switch (self.ErrorLevel)
                    {
                        case ErrorLevels.Info:
                            EditorGUILayout.LabelField(Info, GUILayout.Width(30), GUILayout.Height(30));
                            break;

                        case ErrorLevels.Warning:
                            EditorGUILayout.LabelField(Warn, GUILayout.Width(30), GUILayout.Height(30));
                            break;

                        case ErrorLevels.Error:
                            EditorGUILayout.LabelField(Error, GUILayout.Width(30), GUILayout.Height(30));
                            break;
                    }

                    using (new GUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label(self.Message, EditorStyles.wordWrappedLabel);
                            GUILayout.FlexibleSpace();
                        }

                        if (self.Context.Context != null)
                        {
                            EditorGUILayout.ObjectField(self.Context.Context, self.Context.Type, true);
                        }
                        if (self.Context.Extended != null)
                        {
                            self.Context.Extended();
                        }
                        GUILayout.Space(8);
                    }

                    if (extended != null)
                    {
                        extended();
                    }
                }
            }
        }
        
        #endregion DrawGUI
    }
}