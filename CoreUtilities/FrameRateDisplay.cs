using TMPro;
using UnityEngine;

namespace Utilities
{
    public class FrameRateDisplay : MonoBehaviour
    {
        [Tooltip("The text component to display the frame rate. Will be displayed as FPS: {frame rate}")]
        [SerializeField] private TMP_Text _text;
        
        private void Update()
        {
            _text.text = $"FPS: {1f / Time.deltaTime, 0:F0}";
        }
    }
}