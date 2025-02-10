using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KT.Scripts
{
    public class PlayerAnimator
    {
        private readonly PlayerContainer _playerContainer;
        private string _currentAnimation;

        private Vector3 _currentPlayerMovement;
        private bool _isAttacking; 
        private bool _isCurrentlyRunning;
        private readonly PlayerInteractor _playerInteractor;

        [Inject]
        public PlayerAnimator(PlayerContainer playerContainer, PlayerInteractor playerInteractor)
        {
            _playerContainer = playerContainer;
            _playerInteractor = playerInteractor;
        }

        public void ManageAnimation(Vector3 playerMovement, bool isRunning)
        {
            _currentPlayerMovement = playerMovement;
            _isCurrentlyRunning = isRunning;

            if (_currentAnimation == "")
            {
                HandleJumpAndLandAnimations(playerMovement, isRunning);

                HandleRunSprintIdleAnimations(playerMovement, isRunning);
            }
            

            HandleJumpAndLandAnimations(playerMovement, isRunning);

            HandleRunSprintIdleAnimations(playerMovement, isRunning);
        }

        private void HandleJumpAndLandAnimations(Vector3 playerMovement, bool isRunning)
        {

            if (_playerContainer.IsJumping)
            {
                if (playerMovement is { x: 0, z: 0 }) 
                    ChangeAnimation("Jump_Idle").Forget();
                else if (isRunning) 
                    ChangeAnimation("Jump_Sprinting").Forget();
                else 
                    ChangeAnimation("Jump_Running").Forget();
                return;
            }

      
            if (_currentAnimation is "Jump_Idle" or "Jump_Running" or "Jump_Sprinting" &&
                _playerContainer.PlayerController.isGrounded)
            {
                if (playerMovement is { x: 0, z: 0 }) 
                    ChangeAnimation("Land_Idle").Forget();
                else if (isRunning) 
                    ChangeAnimation("Land_Sprinting").Forget();
                else 
                    ChangeAnimation("Land_Running").Forget();
            }
        }

        private void HandleRunSprintIdleAnimations(Vector3 playerMovement, bool isRunning)
        {
            if (!_playerContainer.IsJumping && _playerContainer.PlayerController.isGrounded)
            {
                if (isRunning && playerMovement is not { x: 0, z: 0 })
                {
                    ChangeAnimation("Sprint_F").Forget();
                }
                else if (!isRunning && playerMovement is not { x: 0, z: 0 })
                {
                    if (playerMovement.z < 0 && playerMovement.x < 0)
                        ChangeAnimation("Run_BckStrafeBL").Forget();
                    else if (playerMovement.z < 0 && playerMovement.x > 0)
                        ChangeAnimation("Run_BckStrafeBR").Forget();
                    else if (playerMovement.x < 0)
                        ChangeAnimation("Run_BckStrafeL").Forget();
                    else if (playerMovement.z > 0 && playerMovement.x < 0)
                        ChangeAnimation("Run_BckStrafeFL").Forget();
                    else if (playerMovement.z > 0 && playerMovement.x > 0)
                        ChangeAnimation("Run_BckStrafeFR").Forget();
                    else if (playerMovement.z < 0)
                        ChangeAnimation("Run_BckStrafeB").Forget();
                    else
                        ChangeAnimation("Run_F").Forget();
                }
                else if (!isRunning && playerMovement is { x: 0, z: 0 })
                {
                    ChangeAnimation("Idle_v1").Forget();
                }
            }
        }
        
        public void OnAnimatorIK(int layerIndex)
        {
            if (_playerInteractor.isInteracting && _playerInteractor.targetObject != null)
            {
                _playerContainer.PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _playerContainer.PlayerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

                _playerContainer.PlayerAnimator.SetIKPosition(AvatarIKGoal.RightHand, _playerInteractor.targetObject.transform.position);
                _playerContainer.PlayerAnimator.SetIKRotation(AvatarIKGoal.RightHand, _playerInteractor.targetObject.transform.rotation);
            }
            else
            {
                _playerContainer.PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _playerContainer.PlayerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }

        public async UniTask ChangeAnimation(string animation, float crossFade = 0.1f, float time = 0f)
        {
            if (time > 0) await UniTask.Delay((int)((time - crossFade) * 1000)); 

            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _playerContainer.PlayerAnimator.CrossFade(animation, crossFade);
            }
        }
    }
}