using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities
{
    public class AnimatorTriggerer : MonoBehaviour
    {
        [Tooltip("The animator that will have the trigger set.")]
        [SerializeField] private Animator _animator;
        [Tooltip("The type of trigger that will be set.")]
        [SerializeField, OnValueChanged("ClearParameterName")] private AnimatorControllerParameterType _triggerType;
        [Tooltip("The name of the trigger that will be set.")]
        [SerializeField, ValueDropdown("GetParameterNames")] private string _triggerName;
        [Tooltip("The value that the trigger will be set to if its a bool.")]
        [SerializeField, ShowIf("_triggerType", AnimatorControllerParameterType.Bool)] private bool _boolValue;
        [Tooltip("The value that the trigger will be set to if its a float.")]
        [SerializeField, ShowIf("_triggerType", AnimatorControllerParameterType.Float)] private float _floatValue;
        [Tooltip("The value that the trigger will be set to if its an int.")]
        [SerializeField, ShowIf("_triggerType", AnimatorControllerParameterType.Int)] private int _intValue;
    
        /// <summary>
        /// Will cause the associated animator to have the specified trigger of type <see cref="_triggerType"/> be triggered
        /// with the specified value, if applicable.
        /// </summary>
        public void Trigger()
        {
            if (_triggerName == "") return;
        
            switch (_triggerType)
            {
                case AnimatorControllerParameterType.Bool:
                    _animator.SetBool(_triggerName, _boolValue);
                    break;
                case AnimatorControllerParameterType.Float:
                    _animator.SetFloat(_triggerName, _floatValue);
                    break;
                case AnimatorControllerParameterType.Int:
                    _animator.SetInteger(_triggerName, _intValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    _animator.SetTrigger(_triggerName);
                    break;
                default:
                    Debug.LogError("Invalid trigger type", gameObject);
                    break;
            }
        }

        private List<string> GetParameterNames()
        {
            return _animator == null ? new List<string>() 
                : _animator.parameters.Where(x => x.type == _triggerType).Select(p => p.name).ToList();
        }

        private void ClearParameterName()
        {
            _triggerName = "";
        }
    }
}