using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
    public class FadeToBlackSystem : SingletonMonoBehavior<FadeToBlackSystem>
    {
        public enum FadeType
        {
            FadeIntoBlack,
            FadeOutOfBlack
        }
        
        [Tooltip("If true, the scene will start with a full black screen.")]
        [SerializeField] private bool _startFullAlpha;
        [Tooltip("If true, the scene will automatically fade out to black at start of a scene.")]
        [SerializeField] private bool _autoFadeOut;
        [Tooltip("The image that will be used to fade in and out.")]
        [SerializeField] private Image _image;

        /// <summary>
        /// Returns true if the screen is fully faded out.
        /// </summary>
        public static bool FadeOutComplete => Instance == null || Instance._image.color.a == 0;

        private void Start()
        {
            SetFadePercentage(_startFullAlpha ? 1 : 0);
            SceneManager.sceneLoaded += CheckForAutoFadeOut;
            CheckForAutoFadeOut(new Scene(), LoadSceneMode.Single);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= CheckForAutoFadeOut;
        }
        
        /// <summary>
        /// Will perform the corresponding fade action over a period of time equal to the specified duration.<br></br>
        /// FadeType.FadeIntoBlack will fade the screen to black.<br></br>
        /// FadeType.FadeOutOfBlack will fade the screen out of black.
        /// </summary>
        public static IEnumerator TryPerformFadeAction(FadeType fadeType, float duration)
        {
            if (Instance == null) yield break;
            
            var fadeIntoBlack = fadeType == FadeType.FadeIntoBlack;
            Instance.SetFadePercentage(fadeIntoBlack ? 0 : 1);
            
            var startTime = Time.realtimeSinceStartup;
            
            while (Time.realtimeSinceStartup - startTime < duration)
            {
                var newAlpha = (Time.realtimeSinceStartup - startTime) / duration;
                Instance.SetFadePercentage(fadeIntoBlack ? newAlpha : 1 - newAlpha);
                yield return null;
            }
            
            Instance.SetFadePercentage(fadeIntoBlack ? 1 : 0);
        }
        
        private void CheckForAutoFadeOut(Scene _, LoadSceneMode __)
        {
            if (!_autoFadeOut) return;
            
            SetFadePercentage(1);
            StartCoroutine(HandleStartOfScene());
        }

        private void SetFadePercentage(float percentage)
        {
            var temp = _image.color;
            temp.a = percentage;
            _image.color = temp;
        }

        private IEnumerator HandleStartOfScene()
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(TryPerformFadeAction(FadeType.FadeOutOfBlack, 1.5f));
        }
    }
}
