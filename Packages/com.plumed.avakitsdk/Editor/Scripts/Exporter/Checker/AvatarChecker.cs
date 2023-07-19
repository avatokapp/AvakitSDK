using System.Collections.Generic;
using UniGLTF;
using UnityEditor;
using UnityEngine;
using VRM;

namespace AvakitSDK.Exporter
{
    public static class AvatarChecker 
    {
        #region Variables

        private static GameObject SelectedAvatar;
        private static List<Validation> _validations = new List<Validation>();

        #endregion Variables
        
        #region Help Method
        
        private static bool CheckAvatarSelected()
        {
            SelectedAvatar = Selection.activeGameObject;
            return SelectedAvatar != null;
        }

        public static bool SetAvatar(GameObject avatar)
        {
            if (avatar == null) return false;
            
            SelectedAvatar = avatar;
            AvatarComponentChecker.SetAvatar(avatar);
            AvatarMaterialChecker.SetAvatar(avatar);
            return true;
        }

        public static bool CheckRoot(ref List<Validation> validations)
        {
            _validations = validations;
            return VRMRootValid ? AvatarSelectPassed : AvatarSelectError;
        }
        private static bool VRMRootValid => CheckVRMMeta & CheckVRMHumanoidDescription & CheckAnimator & CheckVRMBlendShapeProxy;
        private static bool CheckVRMMeta
        {
            get
            {
                if (SelectedAvatar.GetComponent<VRMMeta>() != null) return true;
                
                _validations.Add(Validation.Error(Localization.RequiredMessage("VRM Meta")));
                return false;

            }
        }
        private static bool CheckVRMHumanoidDescription
        {
            get
            {
                if (SelectedAvatar.GetComponent<VRMHumanoidDescription>() != null) return true;
                
                _validations.Add(Validation.Error(Localization.RequiredMessage("VRM Humanoid Description")));
                return false;
            }
        }
        private static bool CheckAnimator
        {
            get
            {
                if (SelectedAvatar.GetComponent<Animator>() != null) return true;
                
                _validations.Add(Validation.Error(Localization.RequiredMessage("Animator")));
                return false;
            }
        }
        private static bool CheckVRMBlendShapeProxy
        {
            get
            {
                if (SelectedAvatar.GetComponent<VRMBlendShapeProxy>() != null) return true;
                
                _validations.Add(Validation.Error(Localization.RequiredMessage("VRM BlendShape Proxy")));
                return false;
            }
        }
        
        #endregion Help Method

        #region Passed

        private static bool AvatarSelectPassed
        {
            get
            {
                _validations.Add(Validation.Info("Root OK"));
                return true;
            }
        }

        #endregion

        #region Error

        private static bool AvatarSelectError
        {
            get
            {
                // Debug.LogError("[Avatar Checker] You must select an avatar in Hierarchy");
                return false;
            }
        }

        #endregion Error
    }
}

