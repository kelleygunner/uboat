using UnityEngine;
using Zenject;

namespace Game.Code.Core.AppRunning
{
    public class AppRunnerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AppRunner>().FromNew().AsSingle().NonLazy();
        }
    }
}
