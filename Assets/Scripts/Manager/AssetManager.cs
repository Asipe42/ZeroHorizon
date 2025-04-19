using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Manager
{
    public class AssetManager
    {
        public void Init()
        {
            
        }
        
        public async UniTask<T> LoadAsset<T>(string key) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key: key);
            
            try
            {
                T result = await handle.ToUniTask();
                Debug.Log($"{key} loaded");
                
                return result;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{key} could not be loaded: {e.Message}");
                return null;
            }
        }

        public async UniTask<T> InstantiateAsset<T>(string key) where T : UnityEngine.Component
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key: key);

            try
            {
                GameObject result = await handle.ToUniTask();
                Debug.Log($"{key} instantiated");
                
                return result.GetComponent<T>();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{key} could not be instantiated");
                return null;
            }
        }
    }
}