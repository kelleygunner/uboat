using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Code.Base.AssetManagement;
using Game.Code.Common.GameLoop.Context;
using UnityEngine;
using Zenject;

namespace Game.Code.Core.GameContext
{
    public class LoadingGameContext : IGameContext
    {
        [Inject] private AssetManager _assetManager;
        
        private List<string> _groupsToPreload = new List<string>()
        {
            "Loading",
        };
        public async UniTask Load()
        {
            var tasks = 
                Enumerable.Select(_groupsToPreload, group => _assetManager.Preload(group)).ToList();
            await UniTask.WhenAll(tasks);
            
            var prefab = _assetManager.GetAsset<GameObject>("LoadingScreen");
            GameObject.Instantiate(prefab);
            
            Debug.Log("Loading Game Context");
        }

        public UniTask Unload()
        {
            Debug.Log("Unloading Game Context");
            return UniTask.CompletedTask;
        }
    }
}
