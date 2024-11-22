using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// This class is a debug class that will allow for testing events through debug statements.
    /// </summary>
    public class ManualLogger : MonoBehaviour
    {
        /// <summary>
        /// Will log to the console the given scream.
        /// </summary>
        public void LogMessage(string message)
        {
            Debug.Log(message);
        }
    }
}