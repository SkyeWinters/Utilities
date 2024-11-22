using UnityEditor;
using UnityEngine;

namespace Utilities.Editor
{
    public class RenderCubemapWizard : ScriptableWizard
    {
        private const string BASE_PATH = "Assets/Skyboxes/";
        private const string BASE_NAME = "NewSkybox";
        private const string CUBEMAP_EXTENSION = ".cubemap";
        private const string MATERIAL_EXTENSION = ".mat";
        private static readonly int Tex = Shader.PropertyToID("_Tex");

        [MenuItem("Tools/SkyboxCreator", priority = 10)]
        private static void RenderCubemap()
        {
            DisplayWizard<RenderCubemapWizard>("Skybox Creator", "Create", "Cancel");
        }
        
        private void OnWizardUpdate()
        {
            isValid = (SceneView.lastActiveSceneView != null);
            helpString = isValid ? "Your good to go!" : "No active scene view found.";
        }

        private void OnWizardCreate()
        {
            var sceneView = SceneView.lastActiveSceneView;
            
            var cubemap = new Cubemap(1024, TextureFormat.RGB24, false);
            var skybox = new Material(Shader.Find("Skybox/Cubemap"));
            skybox.SetTexture(Tex, cubemap);
            
            var currentRotation = sceneView.camera.transform.rotation;
            sceneView.camera.transform.rotation = Quaternion.identity;
            
            sceneView.camera.RenderToCubemap(cubemap);
            
            sceneView.camera.transform.rotation = currentRotation;
            
            if (!AssetDatabase.IsValidFolder("Assets/Skyboxes"))
            {
                AssetDatabase.CreateFolder("Assets", "Skyboxes");
            }
            
            
            SaveSkybox(cubemap, skybox);
        }
        
        private static void SaveSkybox(Cubemap cubemap, Material skybox)
        {
            var cubemapPath = GenerateUniquePath(BASE_PATH, BASE_NAME, CUBEMAP_EXTENSION);
            var materialPath = GenerateUniquePath(BASE_PATH, BASE_NAME, MATERIAL_EXTENSION);
            
            AssetDatabase.CreateAsset(cubemap, cubemapPath);
            AssetDatabase.CreateAsset(skybox, materialPath);

            Debug.Log("Skybox created! Saved to " + cubemapPath + " and " + materialPath);
        }

        private static string GenerateUniquePath(string basePath, string baseName, string extension)
        {
            string fullPath;
            var count = 0;

            do
            {
                fullPath = $"{basePath}{baseName}{(count > 0 ? $" ({count})" : "")}{extension}";
                count++;
            }
            while (System.IO.File.Exists(fullPath));

            return fullPath;
        }
        
        private void OnWizardOtherButton()
        {
            Close();
        }
    }
}