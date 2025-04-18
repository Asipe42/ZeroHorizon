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
        private UIConfig _config;
        
        public async UniTask Init()
        {
            /*
             * Init 과정
             *  - Config 로드
             *  - BaseUI 로드 및 초기화
             */
            
            _config = await GameManager.Instance.Assets.LoadAsset<UIConfig>(key: nameof(UIConfig));
            
            foreach (var kvp in _config.AssetKeys)
            {
                var asset = await GameManager.Instance.Assets.InstantiateAsset<BaseUI>(kvp.Value);
                asset.Init();
                
                _cacheUIs.Add(kvp.Key, asset);
            }
            
            Debug.Log($"Initialize {nameof(UIManager)}");
        }

        public void OpenUI(ClientEnum.EUIType type, BaseUIModel _model)
        {
            /*
             * OpenUI 과정
             *  - UI를 얻는다.
             *  - UI에 모델을 주입한다.
             *  - UI를 연다.
             */

            if (TryFindUI(type, out var ui) == false)
            {
                Debug.LogError($"Unable to find UI matching type: {type}");
                return;
            }

            _model.Init();
            ui.BindModel(_model);
            ui.Open();

            Debug.Log($"Open UI: {type}");
        }
        
        public void CloseUI(ClientEnum.EUIType type)
        {
            /*
             * CloseUI 과정
             *  - UI와 모델을 얻는다.
             *  - UI와 모델을 정리한다.
             *  - UI를 끈다.
             */
            
            if (TryFindUI(type, out var ui) == false)
            {
                Debug.LogError($"Unable to find UI matching type: {type}");
                return;
            }
            
            ui.CleanUp();
            ui.Close();
            
            Debug.Log($"Close UI: {type}");
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