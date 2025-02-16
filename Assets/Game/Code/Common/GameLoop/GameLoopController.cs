using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Code.Common.GameLoop.Context;
using UnityEngine;

namespace Game.Code.Common.GameLoop
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GameLoopController : IGameContextLoadable, IGameLoopRunnable
    {
        private CancellationTokenSource _cts;
        private IGameContext _currentContext;
        private bool _locked;
        private List<IGameContext> _commonContext;
        
        void IGameLoopRunnable.StartLoop()
        {
            _commonContext = new List<IGameContext>();
            _cts = new CancellationTokenSource();
            MainLoop(_cts.Token).Forget();
        }

        void IGameLoopRunnable.StopLoop()
        {
            _cts.Cancel();
        }
        
        async UniTask<bool> IGameContextLoadable.ChangeSingleContext(IGameContext context)
        {
            if (_locked)
            {
                return false;
            }
            _locked = true;
            
            // Signal Loading Screen show
            await _currentContext.Unload();
            await context.Load();
            _currentContext = context;
            // Signal Loading Screen hide
            _locked = false;
            return true;
        }

        async UniTask IGameContextLoadable.AddCommonContext(IGameContext context)
        {
            await context.Load();
            _commonContext.Add(context);
        }
        
        private async UniTask MainLoop(CancellationToken ct)
        {
            Debug.Log("Start Game Loop");
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Yield(ct);
            }

            // Unload all loaded context
            var finishTasks = new List<UniTask> { _currentContext.Unload() };
            finishTasks.AddRange(Enumerable.Select(_commonContext, context => context.Unload()));
            await UniTask.WhenAll(finishTasks);
            Debug.Log("Stop Game Loop");
        }
    }
}
