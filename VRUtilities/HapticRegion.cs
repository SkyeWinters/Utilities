using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR.Haptics;

namespace Utilities
{
    public class HapticRegion : MonoBehaviour
    {
        [Tooltip("The strength at which the controller will vibrate")]
        [SerializeField] private float _strength = 0.5f;
        [Tooltip("The duration of the vibration")]
        [SerializeField] private float _duration = 0.1f;
        [Tooltip("The delay between each pulse")]
        [SerializeField] private float _pulseDelay = 0.5f;
        
        [Header("Events")]
        [SerializeField] private UnityEvent _onEnter;
        [SerializeField] private UnityEvent _onExit;

        /// <summary>
        /// Will return a haptic impulse command that can be used to trigger an impulse on a controller.
        /// </summary>
        public SendHapticImpulseCommand GetHapticCommand()
        {
            return SendHapticImpulseCommand.Create(0, _strength, _duration);
        }

        /// <summary>
        /// Will return the time between the start of one pulse and the start of the next pulse.
        /// </summary>
        public float GetPulseRate()
        {
            return _pulseDelay + _duration;
        }
        
        public void Enter()
        {
            _onEnter.Invoke();
        }
        
        public void Exit()
        {
            _onExit.Invoke();
        }
    }
}