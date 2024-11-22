using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class SceneLoader : MonoBehaviour
    {
        [Tooltip("Is there a specific scene you wish to go to?")]
        [SerializeField] private bool _specificScene;
        [Tooltip("The scene to load"), ShowIf("_specificScene")]
        [SerializeField, ValueDropdown("GetScenes")] private string _gameScene;

        private IEnumerable GetScenes()
        {
            var scenes = new List<string>();
            var totalScenes = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < totalScenes; i++)
            {
                var scene = SceneUtility.GetScenePathByBuildIndex(i).Split("/")[^1].Split(".")[0];
                scenes.Add(scene);
            }
            
            return scenes;
        }
        
        public void LoadGameScene()
        {
            StartCoroutine(_specificScene
                ? SceneTools.TransitionToScene(_gameScene)
                : SceneTools.TransitionToNextScene());
        }
    }
}