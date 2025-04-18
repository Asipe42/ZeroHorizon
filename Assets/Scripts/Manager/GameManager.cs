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
        
        public bool IsInit { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();

            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            IsInit = false;
            
            InitEvent();
            await InitAssetManager();
            await InitUIManager();

            IsInit = true;
        }

        private void InitEvent()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        private async UniTask InitAssetManager()
        {
            Assets = new AssetManager();
            Assets.Init();

            await UniTask.Yield();
        }

        private async UniTask InitUIManager()
        {
            UI = new UIManager();
            await UI.Init();
            
            await UniTask.Yield();
        }
    }
}
