using System;
using Cinemachine;
using UnityEngine;

namespace KT.Scripts
{
    public class CameraSwitcher : MonoBehaviour
    {
        [field: SerializeField] private CinemachineFreeLook FirstPerson;
        [field: SerializeField] private CinemachineVirtualCamera ThirdPerson;
        private ICinemachineCamera _currentCamera;

        private void Start()
        {
            _currentCamera = ThirdPerson;
            _currentCamera.Priority = 10;
        }

        public void SwitchCamera()
        {
            if (_currentCamera == FirstPerson)
            {
                _currentCamera.Priority = 0;
                _currentCamera = ThirdPerson;
                _currentCamera.Priority = 10;
            }
            else
            {
                _currentCamera.Priority = 0;
                _currentCamera = FirstPerson;
                _currentCamera.Priority = 10;
            }
        }
    }
}
