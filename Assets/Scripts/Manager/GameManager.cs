using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Manager
{
    /// <summary>
    /// 프로그램 전반에 대한 제어 및 관리를 담당한다.
    /// </summary>
    public partial class GameManager : MonoSingleton<GameManager>
    {
        public UIManager UI { get; private set; }
        public AssetManager Assets { get; private set; }
        public AuthManager Auth { get; private set; }
        
        public bool IsInit { get; private set; }

        public event Action OnInitAssetManager;
        public event Action OnInitUIManager;
        public event Action OnInitAuthManager;
        
        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public async UniTaskVoid Init()
        {
            IsInit = false;
            
            await InitAssetManager();
            await InitUIManager();
            await InitAuthManager();

            IsInit = true;
        }
        
        private async UniTask InitAssetManager()
        {
            Assets = new AssetManager();
            Assets.Init();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"Complete Initialize {nameof(AssetManager)}");
            
            OnInitAssetManager?.Invoke();
        }

        private async UniTask InitUIManager()
        {
            UI = new UIManager();
            await UI.Init();
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"Complete Initialize {nameof(UIManager)}");
            
            OnInitUIManager?.Invoke();
        }

        private async UniTask InitAuthManager()
        {
            Auth = new AuthManager();
            await Auth.Init();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"Complete Initialize {nameof(AuthManager)}");
            
            OnInitAuthManager?.Invoke();
        }
    }
}
