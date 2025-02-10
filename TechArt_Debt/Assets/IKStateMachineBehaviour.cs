using UnityEngine;
using Zenject;

public class IKStateMachineBehaviour : StateMachineBehaviour
{
    private PlayerInteractor _playerInteractor;

    [Inject]
    public void Construct(PlayerInteractor playerInteractor)
    {
        _playerInteractor = playerInteractor;
    }

    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerInteractor.isInteracting && _playerInteractor.targetObject != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

            animator.SetIKPosition(AvatarIKGoal.RightHand, _playerInteractor.targetObject.transform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, _playerInteractor.targetObject.transform.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }
}