using UnityEngine;

namespace Utilities
{
    public class Screamer : MonoBehaviour
    {
        /// <summary>
        /// Broadcast the message to the debug log.
        /// </summary>
        public void Scream(string message)
        {
            Debug.Log(message);
        }
    }
}