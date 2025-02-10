    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Zenject;

    namespace KT.Scripts
    {
        public class InputListener : MonoBehaviour
        {
            private bool _isRunPressed;
            private MyInputSystem _playerInput;
            private PlayerInvoker _playerInvoker;

            private Vector2 _playerMovement;
            private bool _isJumpPressed;

            private void Update()
            {
                _playerInvoker.ProvideInput(_playerMovement, _isJumpPressed, _isRunPressed);
            }

            private void OnEnable()
            {
                _playerInput.Enable();

                _playerInput.Player.Move.started += OnMove;
                _playerInput.Player.Move.canceled += OnMove;
                _playerInput.Player.Move.performed += OnMove;

                _playerInput.Player.Sprint.performed += OnRun;
                _playerInput.Player.Sprint.canceled += OnRun;
                
                _playerInput.Player.Jump.started += OnJump;
                _playerInput.Player.Jump.canceled += OnJump;
                
                _playerInput.Player.CameraSwitch.started += OnCameraSwitch;
                _playerInput.Player.Interact.started += OnInteract;
            }

            private void OnInteract(InputAction.CallbackContext obj)
            {
                _playerInvoker.Interact();
            }

            private void OnDisable()
            {
                _playerInput.Player.Move.started -= OnMove;
                _playerInput.Player.Move.canceled -= OnMove;
                _playerInput.Player.Move.performed += OnMove;

                _playerInput.Player.Sprint.performed -= OnRun;
                _playerInput.Player.Sprint.canceled -= OnRun;
                
                _playerInput.Player.Jump.started -= OnJump;
                _playerInput.Player.Jump.canceled -= OnJump;
                
                _playerInput.Player.CameraSwitch.started -= OnCameraSwitch;
                _playerInput.Player.Interact.started -= OnInteract;

                _playerInput.Disable();
            }

            private void OnCameraSwitch(InputAction.CallbackContext obj)
            {
                _playerInvoker.SwitchCamera();
            }

            [Inject]
            public void Construct(PlayerInvoker playerInvoker, MyInputSystem myInputSystem)
            {
                _playerInvoker = playerInvoker;
                _playerInput = myInputSystem;
            }

            private void OnRun(InputAction.CallbackContext obj)
            {
                _isRunPressed = obj.ReadValueAsButton();
            }

            private void OnJump(InputAction.CallbackContext obj)
            {
                _isJumpPressed = obj.ReadValueAsButton();
            }

            private void OnMove(InputAction.CallbackContext obj)
            {
                _playerMovement = obj.ReadValue<Vector2>();
            }
            
        }
    }