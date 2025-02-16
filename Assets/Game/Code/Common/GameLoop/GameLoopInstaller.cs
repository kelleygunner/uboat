using Zenject;

namespace Game.Code.Common.GameLoop
{
    public class GameLoopInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameLoopController>().FromNew().AsSingle().NonLazy();
        }
    }
}
