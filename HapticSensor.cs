using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem.XR.Haptics;

namespace Utilities
{
    public class HapticSensor : MonoBehaviour
    {
        [Tooltip("Whether this sensor is on the left hand.")]
        [SerializeField] private bool _isLeftHand;

        private Coroutine _inRegion;
        private SendHapticImpulseCommand _command;
        private float _pulseRate;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<HapticRegion>(out var region)) return;
            
            region.Enter();
            _command = region.GetHapticCommand();
            _pulseRate = region.GetPulseRate();
            _inRegion = StartCoroutine(Pulsate());
        }

        private IEnumerator Pulsate()
        {
            while (true)
            {
                var device = InputSystem.GetDevice<XRController>(_isLeftHand ? CommonUsages.LeftHand : CommonUsages.RightHand);
                device.ExecuteCommand(ref _command);
                yield return new WaitForSeconds(_pulseRate);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<HapticRegion>(out var region)) return;
            
            region.Exit();
            StopCoroutine(_inRegion);
        }
    }
}