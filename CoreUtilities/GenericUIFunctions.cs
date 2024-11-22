using UnityEngine;

namespace Utilities
{
    public class GenericUIFunctions : MonoBehaviour
    {
        /// <summary>
        /// Will load the next scene in the build order.
        /// </summary>
        public void LoadNextScene()
        {
            StartCoroutine(SceneTools.TransitionToScene(SceneTools.NextSceneIndex));
        }
        
        /// <summary>
        /// Will load the scene at the given build index.
        /// </summary>
        public void LoadScene(int index)
        {
            StartCoroutine(SceneTools.TransitionToScene(index));
        }
        
        /// <summary>
        /// Will terminate the application.
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            
            Application.Quit();
        }
    }
}