using Framework.UI.Structure;
using Game.Audio.Structure;
using Game.Input.Structure;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputProvider>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<INavigationProvider>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<IAudioEffectsProvider>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}