using UnityEngine;

namespace Utilities
{
    public class CommonAnimatorEvents : MonoBehaviour
    {
        /// <summary>
        /// Will make the game object visible
        /// </summary>
        public void MakeVisible()
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Will make the game object invisible
        /// </summary>
        public void MakeInvisible()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Will destroy the game object
        /// </summary>
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}