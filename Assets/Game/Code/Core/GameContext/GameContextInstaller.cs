using Zenject;

namespace Game.Code.Core.GameContext
{
    public class GameContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadingGameContext>().FromNew().AsSingle().NonLazy();
        }
    }
}
