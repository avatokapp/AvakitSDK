using System;
using System.Collections.Generic;
using System.Linq;
using UniGLTF;
using UnityEngine;

namespace AvakitSDK.Exporter
{
    public static class AvatarMaterialChecker 
    {
        #region Variables

        private static GameObject SelectedAvatar;
        
        private static Renderer[] _renderers;
        private static List<Material> _materials;
        
        private static List<(Validation, Material)> _validations = new List<(Validation, Material)>();
        private static List<Material> _invalidMaterials = new List<Material>();

        #endregion Variables
        
        #region Help Method

        public static void SetAvatar(GameObject avatar) => SelectedAvatar = avatar;

        public static bool CheckMaterials(ref List<(Validation, Material)> validations)
        {
            _validations = validations;
            return ShaderValid ? ShaderCheckPassed : ShaderCheckError;
        }
        
        private static bool ShaderValid => CheckShaders;
        private static bool CheckShaders
        {
            get
            {
                if (!(SetRenderers() && SetMaterials())) return false;

                _invalidMaterials.Clear();
                foreach (var m in _materials)
                {
                    var gltfMaterial = SupportedShaderNames.Contains(m.shader.name) ? "valid" : "";
                    if (string.IsNullOrEmpty(gltfMaterial))
                    {
                        _validations.Add((Validation.Error(Localization.NotSupportedMessage(m.shader.name), ValidationContext.Create(m)), m));
                        _invalidMaterials.Add(m);
                    }
                }

                return _invalidMaterials.Count == 0;
            }
        }

        public static void RemoveInvalidMaterials(List<(Validation, Material)> validations)
        {
            foreach (var validation in validations)
            {
                ChangeInvalidShaderToDefault(validation.Item2);
            }
        }
        public static void ChangeInvalidShaderToDefault(Material material)
        {
            material.shader = Shader.Find(DefaultShader);
        }

        private static bool SetRenderers()
        {
            if (SelectedAvatar == null) return false;
            
            _renderers = SelectedAvatar.transform.GetComponentsInChildren<Renderer>(true);
            return true;
        }

        private static bool SetMaterials()
        {
            if (_renderers == null) return false;

            var renderers = _renderers;
            var tempDictionary = new Dictionary<Material, Action>();
            var materials = new List<Material>();

            foreach (var renderer in renderers)
            {
                var mats = renderer.sharedMaterials;
                foreach (var mat in mats)
                {
                    tempDictionary.TryAdd(mat, () => materials.Add(mat));
                }
            }

            foreach (var value in tempDictionary)
            {
                value.Value.Invoke();
            }
    
            _materials = materials;
            return true;
        }

        #endregion Help Method

        #region Passed
        
        private static bool ShaderCheckPassed
        {
            get
            {
                _validations.Add((Validation.Info("Shaders OK"), null));
                return true;
            }
        }

        #endregion

        #region Error

        private static bool ShaderCheckError
        {
            get
            {
                // Debug.LogError("[Avatar Checker] Unsupported components must be deleted from the avatar");
                return false;
            }
        }

        #endregion Error
        
        #region SupportedShader

        private static readonly string DefaultShader = "lilToon";

        private static readonly string[] SupportedShaderNames =
        {
            // UniGLTF & VRM default shader
            "UniGLTF/UniUnlit",
            "VRM/MToon",
            // "VRM10/MToon10",
            // Unity default shader
            "Standard",
            "Unlit/Color",
            "Unlit/Texture",
            "Unlit/Transparent",
            "Unlit/Transparent Cutout",
            "Sprites/Default",
            "Particles/Standard Unlit",
            "Legacy Shaders/Bumped Diffuse",
            "Legacy Shaders/Bumped Specular",
            "Legacy Shaders/Decal",
            "Legacy Shaders/Diffuse",
            "Legacy Shaders/Diffuse Detail",
            "Legacy Shaders/Diffuse Fast",
            "Legacy Shaders/Lightmapped/Bumped Diffuse",
            "Legacy Shaders/Lightmapped/Bumped Specular",
            "Legacy Shaders/Lightmapped/Diffuse",
            "Legacy Shaders/Lightmapped/Specular",
            "Legacy Shaders/Lightmapped/VertexLit",
            "Legacy Shaders/Parallax Diffuse",
            "Legacy Shaders/Parallax Specular",
            "Legacy Shaders/Particles/Additive",
            "Legacy Shaders/Particles/Additive (Soft)",
            "Legacy Shaders/Particles/Alpha Blended",
            "Legacy Shaders/Particles/Alpha Blended Premultiply",
            "Legacy Shaders/Particles/Anim Alpha Blended",
            "Legacy Shaders/Particles/Multiply",
            "Legacy Shaders/Particles/Multiply (Double)",
            "Legacy Shaders/Particles/VertexLit Blended",
            "Legacy Shaders/Reflective/Bumped Diffuse",
            "Legacy Shaders/Reflective/Bumped Specular",
            "Legacy Shaders/Reflective/Bumped Unlit",
            "Legacy Shaders/Reflective/Bumped VertexLit",
            "Legacy Shaders/Reflective/Diffuse",
            "Legacy Shaders/Reflective/Parallax Diffuse",
            "Legacy Shaders/Reflective/Parallax Specular",
            "Legacy Shaders/Reflective/Specular",
            "Legacy Shaders/Reflective/VertexLit",
            "Legacy Shaders/Self-Illumin/Bumped Diffuse",
            "Legacy Shaders/Self-Illumin/Bumped Specular",
            "Legacy Shaders/Self-Illumin/Diffuse",
            "Legacy Shaders/Self-Illumin/Parallax Diffuse",
            "Legacy Shaders/Self-Illumin/Parallax Specular",
            "Legacy Shaders/Self-Illumin/Specular",
            "Legacy Shaders/Self-Illumin/VertexLit",
            "Legacy Shaders/Specular",
            "Legacy Shaders/Transparent/Bumped Diffuse",
            "Legacy Shaders/Transparent/Bumped Specular",
            "Legacy Shaders/Transparent/Cutout",
            "Legacy Shaders/Transparent/Diffuse",
            "Legacy Shaders/Transparent/Parallax Diffuse",
            "Legacy Shaders/Transparent/Parallax Specular",
            "Legacy Shaders/Transparent/Specular",
            "Legacy Shaders/Transparent/VertexLit",
            "Legacy Shaders/VertexLit",
            // URP shader
            "Universal Render Pipeline/2D/Sprite-Lit-Default",
            "Universal Render Pipeline/2D/Sprite-Mask",
            "Universal Render Pipeline/2D/Sprite-Unlit-Default",
            "Universal Render Pipeline/Particles/Lit",
            "Universal Render Pipeline/Particles/Simple Lit",
            "Universal Render Pipeline/Particles/Unlit",
            "Universal Render Pipeline/Baked Lit",
            "Universal Render Pipeline/Complex Lit",
            "Universal Render Pipeline/Simple Lit",
            "Universal Render Pipeline/Unlit",
            // lilToon shader
            "lilToon",
            "Hidden/lilToonCutout",
            "Hidden/lilToonCutoutOutline",
            "Hidden/lilToonGem",
            "Hidden/lilToonOutline",
            "Hidden/lilToonRefraction",
            "Hidden/lilToonRefractionBlur",
            "Hidden/lilToonFur",
            "Hidden/lilToonFurCutout",
            "Hidden/lilToonFurTwoPass",
            "Hidden/lilToonTransparentOutline",
            "Hidden/lilToonOnePassTransparent",
            "Hidden/lilToonOnePassTransparentOutline",
            "Hidden/lilToonTwoPassTransparent",
            "Hidden/lilToonTwoPassTransparentOutline",
            "Hidden/lilToonTransparent",
            "_lil/lilToonMulti",
            "_lil/lilToonFakeShadow",
            "_lil/[Optional] lilToonFakeShadow",
            "_lil/[Optional] lilToonFurOnly",
            "_lil/[Optional] lilToonFurOnlyCutout",
            "_lil/[Optional] lilToonFurOnlyTransparent",
            "_lil/[Optional] lilToonFurOnlyTwoPass",
            "_lil/[Optional] lilToonLiteOverlay",
            "_lil/[Optional] lilToonLiteOverlayOnePass",
            "_lil/[Optional] lilToonOutlineOnly",
            "_lil/[Optional] lilToonOutlineOnlyCutout",
            "_lil/[Optional] lilToonOutlineOnlyTransparent",
            "_lil/[Optional] lilToonCutoutOutlineOnly",
            "_lil/[Optional] lilToonTransparentOutlineOnly",
            "_lil/[Optional] lilToonOverlay",
            "_lil/[Optional] lilToonOverlayOnePass",
        };
        
        #endregion SupportedShader
    }
}

