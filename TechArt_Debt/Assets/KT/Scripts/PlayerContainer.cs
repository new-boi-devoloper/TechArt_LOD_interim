using UnityEngine;

namespace KT.Scripts
{
    public class PlayerContainer : MonoBehaviour
    {
        [field: SerializeField] public float PlayerSprintSpeed { get; private set; }
        [field: SerializeField] public float PlayerWalkSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        public CharacterController PlayerController { get; private set; }
        public Animator PlayerAnimator { get; private set; }

        public bool IsJumping { get; set; }
        public bool isInteracting { get; set; }
        public Camera PlayerCamera { get; set; }

        private void Start()
        {
            PlayerCamera = Camera.main;
            PlayerController = GetComponent<CharacterController>();
            PlayerAnimator = GetComponent<Animator>();
        }
    }
}