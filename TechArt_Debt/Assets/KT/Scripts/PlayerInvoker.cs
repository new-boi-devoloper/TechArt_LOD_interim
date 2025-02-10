using UnityEngine;
using Zenject;

namespace KT.Scripts
{
    public class PlayerInvoker
    {
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;
        private readonly CameraSwitcher _cameraSwitcher;
        private readonly PlayerInteractor _playerInteractor;

        [Inject]
        public PlayerInvoker(
            PlayerMovement playerMovement,
            PlayerAnimator playerAnimator,
            CameraSwitcher cameraSwitcher,
            PlayerInteractor playerInteractor)
        {
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
            _cameraSwitcher = cameraSwitcher;
            _playerInteractor = playerInteractor;
        }

        public void ProvideInput(Vector2 playerMovement, bool isJumpPressed, bool isRunPressed)
        {
            _playerMovement.Move(playerMovement, isRunPressed);
            _playerMovement.Jump(isJumpPressed);
            _playerAnimator.ManageAnimation(playerMovement, isRunPressed);
        }

        public void SwitchCamera()
        {
            _cameraSwitcher.SwitchCamera();
        }

        public void Interact()
        {
            GameObject interactionObject;
            _playerInteractor.Interact(out interactionObject);

            if (interactionObject != null)
            {
                Debug.Log("Interacting with: " + interactionObject.name);
                _playerAnimator.OnAnimatorIK(1);
            }
        }
    }
}