using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    [RequireComponent(typeof(Collider))]
    public class PlayerAreaTriggerEvents : MonoBehaviour
    { 
        [Tooltip("Event invoked when an object with the player tag enters the trigger.")]
        [SerializeField] private UnityEvent _onEnter;
        [Tooltip("Event invoked when an object with the player tag exits the trigger.")]
        [SerializeField] private UnityEvent _onExit;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _onEnter.Invoke();
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _onExit.Invoke();
            }
        }
    }
}