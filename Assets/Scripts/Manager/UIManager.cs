using System.Collections.Generic;
using Config;
using Cysharp.Threading.Tasks;
using Define;
using UI;
using UnityEngine;

namespace Manager
{
    public class UIManager
    {
        private readonly Dictionary<ClientEnum.EUIType, BaseUI> _cacheUIs = new();
        private readonly Dictionary<ClientEnum.EUIType, BaseUIModel> _cacheUIModels = new();
        private UIConfig _config;
        
        public async UniTask Init()
        {
            /*
             * 초기화 과정
             *  1. Config 로드
             *  2. BaseUI 로드 및 초기화
             */
            
            // No.1
            _config = await GameManager.Instance.Assets.LoadAsset<UIConfig>(key: nameof(UIConfig));
            
            // No.2
            foreach (var kvp in _config.AssetKeys)
            {
                var asset = await GameManager.Instance.Assets.InstantiateAsset<BaseUI>(kvp.Value);
                asset.gameObject.SetActive(false);
                asset.Init();
                _cacheUIs.Add(kvp.Key, asset);
            }
        }

        public void OpenUI(ClientEnum.EUIType type)
        {
            if (TryFindModel(type, out var model) == false)
            {
                
            }
            
            model.Open();
            Debug.Log($"Showing UI: {type}");
        }
        
        public void CloseUI(ClientEnum.EUIType type)
        {
            if (TryFindModel(type, out var model) == false)
            {
                Debug.LogWarning("UI not found: " + type);
                return;
            }
            
            model.Close();
            Debug.Log($"Closing UI: {type}");
        }
        
        public bool TryFindModel(ClientEnum.EUIType type, out BaseUIModel result)
        {
            result = null;
            
            if (_cacheUIModels.TryGetValue(type, out result) == false)
            {
                return false;
            }
            
            return result != null;
        }

        private bool TryFindUI(ClientEnum.EUIType type, out BaseUI result)
        {
            result = null;
            if (_cacheUIs.TryGetValue(type, out result) == false)
            {
                return false;
            }

            return result != null;
        }
    }
}