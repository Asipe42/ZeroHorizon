using System;
using Cysharp.Threading.Tasks;
using Define;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Manager
{
    public partial class GameManager : MonoSingleton<GameManager>
    {
        public UIManager UI { get; private set; }
        public AssetManager Assets { get; private set; }
        public InputManager Input { get; private set; }
        public FirebaseManager Firebase { get; private set; }
        
        public bool IsInit { get; private set; }

        public event Action OnInitAssetManager;
        public event Action OnInitUIManager;
        public event Action OnInitInputManager;
        public event Action<AuthState> OnInitAuthManager;
        
        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            Input.CustomUpdate();
        }

        public async UniTaskVoid Init()
        {
            IsInit = false;
            
            await InitAssetManager();
            await InitUIManager();
            await InitInputManager();
            await InitFirebaseManager();

            switch (Firebase.AuthState)
            {
                case AuthState.None:
                case AuthState.HasUID:
                {
                    break;
                }

                case AuthState.HasUserInfo:
                {
                    LoadScene(SceneType.Main);
                    break;
                }
                
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

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

        private async UniTask InitInputManager()
        {
            Input = new InputManager();
            await Input.Init();
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"Complete Initialize {nameof(InputManager)}");
            
            OnInitInputManager?.Invoke();
        }

        private async UniTask InitFirebaseManager()
        {
            Firebase = new FirebaseManager();
            await Firebase.Init();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"Complete Initialize {nameof(FirebaseManager)}");
            
            OnInitAuthManager?.Invoke(Firebase.AuthState);
        }
    }
}
