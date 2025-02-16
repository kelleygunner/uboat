using Zenject;

namespace Game.Code.Base.AssetManagement
{
    public class AssetManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AssetManager>().FromNew().AsSingle().NonLazy();
        }
    }
}
