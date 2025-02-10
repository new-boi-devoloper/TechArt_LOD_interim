using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace KT.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        [field: SerializeField] private InputListener inputListener;
        [field: SerializeField] private PlayerContainer playerContainer;
        [field: SerializeField] private CameraSwitcher cameraSwitcher;
        [SerializeField] private PlayerInteractor playerInteractor;

        public override void InstallBindings()
        {
            Container.Bind<InputListener>().FromInstance(inputListener).AsSingle().NonLazy();
            Container.Bind<PlayerContainer>().FromInstance(playerContainer).AsSingle().Lazy();
            Container.Bind<CameraSwitcher>().FromInstance(cameraSwitcher).AsSingle().Lazy();
            Container.Bind<PlayerMovement>().AsSingle().Lazy();
            Container.Bind<PlayerInvoker>().AsSingle().Lazy();
            Container.Bind<PlayerAnimator>().AsSingle().Lazy();
            Container.Bind<MyInputSystem>().AsSingle().Lazy();
            Container.Bind<PlayerInteractor>().FromInstance(playerInteractor).AsSingle().Lazy();
        }
    }
}