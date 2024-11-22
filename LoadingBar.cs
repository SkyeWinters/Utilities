using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class LoadingBar : MonoBehaviour
    {
        [Tooltip("The loading bar that will be used to display the progress of the scan.")]
        [SerializeField] private GameObject _loadingBar;
        [Tooltip("The loading bar's progress")]
        [SerializeField] private Image _loadingBarProgress;
    
        private void Start()
        {
            _loadingBar.SetActive(false);
        }
    
        /// <summary>
        /// Will turn the loading bar on and set the progress to 0.
        /// </summary>
        public void Enable()
        {
            _loadingBarProgress.fillAmount = 0;
            _loadingBar.SetActive(true);
        }
    
        /// <summary>
        /// Will set the progress of the loading bar to the given progress.
        /// </summary>
        /// <param name="progress">A normalized value representing the amount of progress made</param>
        public void UpdateProgress(float progress)
        {
            _loadingBarProgress.fillAmount = progress;
        }

        /// <summary>
        /// Will turn the loading bar off.
        /// </summary>
        public void Disable()
        {
            _loadingBar.SetActive(false);
        }
    }
}