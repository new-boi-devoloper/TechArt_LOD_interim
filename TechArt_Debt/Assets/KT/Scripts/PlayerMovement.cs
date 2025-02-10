using UnityEngine;
using Zenject;

namespace KT.Scripts
{
    public class PlayerMovement
    {
        // Параметры прыжка
        private readonly float _maxJumpHeight = 5.0f; // Максимальная высота прыжка
        private readonly float _maxJumpTime = 1.5f; // Время прыжка
        private readonly PlayerContainer _playerContainer;
        private float _gravity; // Гравитация, рассчитанная на основе времени и высоты прыжка
        private float _initialJumpVelocity; // Начальная скорость прыжка
        private bool _isJumping;
        private Vector3 _velocity; // Для обработки гравитации и прыжков

        [Inject]
        public PlayerMovement(PlayerContainer playerContainer)
        {
            _playerContainer = playerContainer;
            SetupJumpVariables(); // Инициализация параметров прыжка
        }

        public void Move(Vector2 moveInput, bool isRunPressed)
        {
            // Применяем гравитацию
            ApplyGravity();

            // Преобразуем входные данные в движение относительно камеры
            var moveDirection = CalculateMovementRelativeToCamera(moveInput);

            // Движение по горизонтали
            var speed = isRunPressed
                ? _playerContainer.PlayerSprintSpeed
                : _playerContainer.PlayerWalkSpeed;
            var move = moveDirection * (Time.deltaTime * speed);

            // Применяем движение через CharacterController
            _playerContainer.PlayerController.Move(move + _velocity * Time.deltaTime);

            // Поворачиваем персонажа в направлении движения
            if (moveDirection != Vector3.zero) RotatePlayerTowardsMovement(moveDirection);
        }

        public void Jump(bool isJumpPressed)
        {
            if (isJumpPressed && _playerContainer.PlayerController.isGrounded && !_isJumping)
            {
                // Применяем начальную скорость для прыжка
                _velocity.y = _initialJumpVelocity;
                _isJumping = true;
                _playerContainer.IsJumping = true;
            }
            else if (_playerContainer.PlayerController.isGrounded && _isJumping)
            {
                // Сбрасываем состояние прыжка, когда персонаж снова на земле
                _isJumping = false;
                _playerContainer.IsJumping = false;
            }
        }

        private void ApplyGravity()
        {
            if (_playerContainer.PlayerController.isGrounded && _velocity.y < 0)
                // Небольшая сила, чтобы "прижать" персонажа к земле
                _velocity.y = -2f;
            else
                // Применяем гравитацию
                _velocity.y += _gravity * Time.deltaTime;

            // Debug.Log($"_playerContainer: IsJumping? {_playerContainer.IsJumping}, Grounded? {_playerContainer.PlayerController.isGrounded}\n PlayerMovement: _isJumping? {_isJumping}");
        }

        private void SetupJumpVariables()
        {
            // Рассчитываем гравитацию и начальную скорость прыжка
            var timeToApex = _maxJumpTime / 2; // Время до достижения максимальной высоты
            _gravity = -2 * _maxJumpHeight / Mathf.Pow(timeToApex, 2); // Формула гравитации
            _initialJumpVelocity = 2 * _maxJumpHeight / timeToApex; // Формула начальной скорости
        }

        private Vector3 CalculateMovementRelativeToCamera(Vector2 moveInput)
        {
            // Получаем направление камеры
            var cameraTransform = _playerContainer.PlayerCamera.transform;

            // Рассчитываем движение относительно камеры
            var moveDirection = cameraTransform.forward * moveInput.y + cameraTransform.right * moveInput.x;
            moveDirection.y = 0; // Игнорируем вертикальную составляющую
            moveDirection.Normalize(); // Нормализуем вектор, чтобы избежать ускорения при диагональном движении

            return moveDirection;
        }

        private void RotatePlayerTowardsMovement(Vector3 moveDirection)
        {
            // Плавно поворачиваем персонажа в направлении движения
            var targetRotation = Quaternion.LookRotation(moveDirection);
            _playerContainer.transform.rotation = Quaternion.Slerp(
                _playerContainer.transform.rotation,
                targetRotation,
                _playerContainer.RotationSpeed * Time.deltaTime
            );
        }
    }
}