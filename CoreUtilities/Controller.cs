using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class Controller : SingletonMonoBehavior<Controller>
    {
        public enum PlayerBinding {PlayerOne, PlayerTwo}

        [Tooltip("The player input component that will be used to handle input registering")]
        [SerializeField] private PlayerInput _playerInput;
        [Tooltip("The controller mapper that will be used to handle key mapping")]
        [SerializeField] private ControllerMapper _controllerMapper;
        
        private static Action<PlayerBinding, bool> _onNotePressed;
        
        /// <summary>
        /// Will invoke an event when a note is pressed. The bool will be true if the note is pressed, false if it is released.
        /// The PlayerBinding will be the player that pressed the note.
        /// </summary>
        public static event Action<PlayerBinding, bool> OnNotePressed
        {
            add => SubscribeEvent(ref _onNotePressed, value);
            remove => RemoveEvent(ref _onNotePressed, value);
        }
        
        /// <summary>
        /// The controller mapper that handles rebinding keys and seeing the current key bindings
        /// </summary>
        public ControllerMapper ControllerMapper => _controllerMapper;

        private void Reset()
        {
            _playerInput = GetComponent<PlayerInput>();
            _controllerMapper = GetComponent<ControllerMapper>();
        }

        private void Start()
        {
            SceneManager.activeSceneChanged += ActivateInput;
            SceneTools.OnSceneTransitionStart += DeactivateInput;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.activeSceneChanged -= ActivateInput;
            SceneTools.OnSceneTransitionStart -= DeactivateInput;
        }

        /// <summary>
        /// Will invoke an input event for player one
        /// </summary>
        /// <param name="context"></param>
        public void OnPlayerOneInput(InputAction.CallbackContext context)
        {
            BroadcastNote(context, PlayerBinding.PlayerOne);
        }
        
        /// <summary>
        /// Will invoke an input event for player two
        /// </summary>
        public void OnPlayerTwoInput(InputAction.CallbackContext context)
        {
            BroadcastNote(context, PlayerBinding.PlayerTwo);
        }
        
        private void ActivateInput(Scene _, Scene __)
        {
            _playerInput.enabled = true;
        }
        
        private void DeactivateInput(int _)
        {
            _playerInput.enabled = false;
        }
        
        private static void SubscribeEvent<T>(ref T caller, T receiver) where T : Delegate
        {
            if (caller == null || !caller.GetInvocationList().Contains(receiver))
            {
                caller = (T)Delegate.Combine(caller, receiver);
            }
        }
        
        private static void RemoveEvent<T>(ref T caller, T receiver) where T : Delegate
        {
            if (caller != null && caller.GetInvocationList().Contains(receiver))
            {
                caller = (T)Delegate.Remove(caller, receiver);
            }
        }

        private static void BroadcastNote(InputAction.CallbackContext context, PlayerBinding type)
        {
            if (context.started)
            {
                _onNotePressed?.Invoke(type, true);
            }
            else if (context.canceled)
            {
                _onNotePressed?.Invoke(type, false);
            }
        }
    }
}
