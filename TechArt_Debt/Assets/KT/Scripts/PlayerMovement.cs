using UnityEngine;
using Zenject;

namespace KT.Scripts
{
    public class PlayerMovement
    {
        private readonly float _maxJumpHeight = 5.0f; 
        private readonly float _maxJumpTime = 1.5f; 
        private readonly PlayerContainer _playerContainer;
        private float _gravity; 
        private float _initialJumpVelocity;
        private bool _isJumping;
        private Vector3 _velocity; 

        [Inject]
        public PlayerMovement(PlayerContainer playerContainer)
        {
            _playerContainer = playerContainer;
            SetupJumpVariables();
        }

        public void Move(Vector2 moveInput, bool isRunPressed)
        {
            ApplyGravity();
            
            var moveDirection = CalculateMovementRelativeToCamera(moveInput);

            var speed = isRunPressed
                ? _playerContainer.PlayerSprintSpeed
                : _playerContainer.PlayerWalkSpeed;
            var move = moveDirection * (Time.deltaTime * speed);

            _playerContainer.PlayerController.Move(move + _velocity * Time.deltaTime);

            if (moveDirection != Vector3.zero) RotatePlayerTowardsMovement(moveDirection);
        }

        public void Jump(bool isJumpPressed)
        {
            if (isJumpPressed && _playerContainer.PlayerController.isGrounded && !_isJumping)
            {
                _velocity.y = _initialJumpVelocity;
                _isJumping = true;
                _playerContainer.IsJumping = true;
            }
            else if (_playerContainer.PlayerController.isGrounded && _isJumping)
            {
                _isJumping = false;
                _playerContainer.IsJumping = false;
            }
        }

        private void ApplyGravity()
        {
            if (_playerContainer.PlayerController.isGrounded && _velocity.y < 0)
                _velocity.y = -2f;
            else
                _velocity.y += _gravity * Time.deltaTime;

        }

        private void SetupJumpVariables()
        {
            var timeToApex = _maxJumpTime / 2; 
            _gravity = -2 * _maxJumpHeight / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * _maxJumpHeight / timeToApex; 
        }

        private Vector3 CalculateMovementRelativeToCamera(Vector2 moveInput)
        {
            var cameraTransform = _playerContainer.PlayerCamera.transform;

            var moveDirection = cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x;
            moveDirection.y = 0; 
            moveDirection.Normalize();

            return moveDirection;
        }

        private void RotatePlayerTowardsMovement(Vector3 moveDirection)
        {
            var targetRotation = Quaternion.LookRotation(moveDirection);
            _playerContainer.transform.rotation = Quaternion.Slerp(
                _playerContainer.transform.rotation,
                targetRotation,
                _playerContainer.RotationSpeed * Time.deltaTime
            );
        }
    }
}