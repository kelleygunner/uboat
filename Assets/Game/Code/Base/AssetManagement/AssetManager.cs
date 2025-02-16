using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Game.Code.Base.AssetManagement
{
    public class AssetManager : IInitializable, IDisposable
    {
        private readonly Dictionary<string, Dictionary<string, object>> _loadedAssets = new();

        public void Initialize()
        {
            Debug.Log("AssetManager Initialized");
        }

        public void Dispose()
        {
            foreach (var group in _loadedAssets.Keys)
            {
                UnloadGroup(group);
            }
            _loadedAssets.Clear();
            Debug.Log("AssetManager Disposed");
        }

        /// <summary>
        /// Загружает все ассеты из указанной Addressables-группы
        /// </summary>
        public async UniTask Preload(string group)
        {
            if (_loadedAssets.ContainsKey(group))
            {
                Debug.LogWarning($"Группа {group} уже загружена.");
                return;
            }

            var assets = new Dictionary<string, object>();
            var handle = Addressables.LoadResourceLocationsAsync(group);

            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Ошибка загрузки группы {group}");
                return;
            }

            List<Task> loadTasks = new();
            foreach (var location in handle.Result)
            {
                var loadHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(location);
                loadTasks.Add(loadHandle.Task.ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        assets[location.PrimaryKey] = t.Result;
                    }
                }));
            }

            await Task.WhenAll(loadTasks);
            _loadedAssets[group] = assets;
            Debug.Log($"Группа {group} загружена ({assets.Count} объектов)");
        }

        /// <summary>
        /// Получает загруженный ассет по имени и группе
        /// </summary>
        public T GetAsset<T>(string asset, string group = "") where T : UnityEngine.Object
        {
            if (group == string.Empty)
            {
                foreach (var groupAssets in _loadedAssets.Values)
                {
                    if (groupAssets.TryGetValue(asset, out var obj) && obj is T casted)
                        return casted;
                }
            }
            else if (_loadedAssets.TryGetValue(group, out var assets) && assets.TryGetValue(asset, out var obj) && obj is T casted)
            {
                return casted;
            }

            Debug.LogWarning($"Ассет {asset} не найден в группе {group}");
            return null;
        }

        /// <summary>
        /// Выгружает все ассеты из указанной группы
        /// </summary>
        public void UnloadGroup(string group)
        {
            if (!_loadedAssets.ContainsKey(group))
            {
                Debug.LogWarning($"Группа {group} не найдена среди загруженных");
                return;
            }

            foreach (var asset in _loadedAssets[group].Values)
            {
                Addressables.Release(asset);
            }

            _loadedAssets.Remove(group);
            Debug.Log($"Группа {group} выгружена");
        }
    }
}
