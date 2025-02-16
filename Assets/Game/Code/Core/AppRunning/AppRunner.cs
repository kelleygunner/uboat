using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Code.Common.GameLoop;
using Game.Code.Common.GameLoop.Context;
using Game.Code.Core.GameContext;
using UnityEngine;
using Zenject;

namespace Game.Code.Core.AppRunning
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AppRunner : IInitializable, IDisposable
    {
        [Inject] private IGameContextLoadable _gameContextLoadable;
        [Inject] private IGameLoopRunnable _gameLoop;
        [Inject] private LoadingGameContext _loadingGameContext;

        public void Initialize()
        {
            Debug.Log("AppRunner Initializing");
            _gameLoop.StartLoop();
            PrepareAppRunning().Forget();
        }

        private async UniTask PrepareAppRunning()
        {
            var tasks = new List<UniTask>();
            tasks.Add(_gameContextLoadable.AddCommonContext(_loadingGameContext));
            
            await UniTask.WhenAll(tasks);
            Debug.Log("App is ready for running game");
        }

        public void Dispose()
        {
            _gameLoop.StopLoop();
        }
    }
}