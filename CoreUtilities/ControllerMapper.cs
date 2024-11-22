using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities
{
    [RequireComponent(typeof(Controller))]
    [RequireComponent(typeof(PlayerInput))]
    public class ControllerMapper : MonoBehaviour
    {
        [Tooltip("The player input component to get the key bindings from")]
        [SerializeField] private PlayerInput _playerInput;

        private void Reset()
        {
            _playerInput = GetComponent<PlayerInput>();
        }
        
        /// <summary>
        /// Returns the name of the key bound to the action with the given name. Optionally provided a display option.
        /// </summary>
        public string GetKey(string keyName, InputBinding.DisplayStringOptions displayOption = 0)
        {
            try
            {
                var controller = Gamepad.current != null ? "Gamepad" : "Keyboard";
                return _playerInput.actions.FindAction(keyName).bindings
                    .Where(x => x.path.Contains(controller))
                    .Select(x => x.ToDisplayString(displayOption))
                    .First();
            }
            catch { Debug.LogError("Can't find key " + keyName); return "?"; }
        }
        
        /// <summary>
        /// Returns the name of the key bound to the action with the given name. Uses the long display name.<br></br>
        /// <i>Same as calling GetKey(keyName, InputBinding.DisplayStringOptions.DontUseShortDisplayNames)</i>
        /// </summary>
        public string GetLongKey(string keyName)
        {
            return GetKey(keyName, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
        }

        /// <summary>
        /// Will perform a rebind operation for the given key name. Will wait until the operation is completed.
        /// Will also disable input while the operation is running.
        /// </summary>
        public IEnumerator AllowUserToSetKey(string keyName)
        {
            _playerInput.enabled = false;
            var operation = _playerInput.actions.FindAction(keyName).PerformInteractiveRebinding();
            
            Debug.Log("Press a key");
            operation.Start();
            yield return new WaitUntil(() => operation.completed);
            operation.Dispose();
            _playerInput.enabled = true;
        }
    }
}