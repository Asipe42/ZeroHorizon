using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Manager
{
    public class AssetManager
    {
        public void LoadAsset<T>(string key, Action<T> onComplete = null) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key: key);
            handle.Completed += (x) =>
            {
                if (x.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogWarning($"File {key} could not be loaded");
                    return;
                }
                
                Debug.Log($"File {key} loaded");
                onComplete?.Invoke(x.Result);
            };
        }
    }
}