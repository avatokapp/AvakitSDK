using System;
using UniGLTF;
using UnityEditor;
using UnityEngine;

namespace AvakitSDK.UI
{
    public class InformationDrawer
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

        public static void DrawGUI(ErrorLevels levels, string message, Action extended = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.HorizontalScope())
                {
                    switch (levels)
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

                    DrawGUI(message);

                    if (extended != null)
                    {
                        extended();
                    }
                }
            }
        }
        
        public static void DrawGUI(string message, Action extended = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.HorizontalScope())
                {
                    DrawGUI(message);

                    if (extended != null)
                    {
                        extended();
                    }
                }
            }
        }

        private static void DrawGUI(string message)
        {
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
        
        #endregion DrawGUI
    }
}