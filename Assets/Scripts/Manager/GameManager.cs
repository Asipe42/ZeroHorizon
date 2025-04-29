using System;
using System.Collections.Generic;
using Actor;
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
        public GamePlayManager GamePlay { get; private set; }
        public FirebaseManager Firebase { get; private set; }
        
        public bool IsInit { get; private set; }

        public event Action OnInitAssetManager;
        public event Action OnInitUIManager;
        public event Action OnInitInputManager;
        public event Action OnInitGamePlayManager;
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
            UpdateGamePlay();
        }

        private void FixedUpdate()
        {
            FixedUpdateGamePlay();
        }

        private void UpdateInput()
        {
            if (IsInit == false)
            {
                return;
            }
            
            Input.CustomUpdate();
        }

        private void UpdateGamePlay()
        {
            if (IsInit == false)
            {
                return;
            }
            
            GamePlay.CustomUpdate();
        }

        private void FixedUpdateGamePlay()
        {
            if (IsInit == false)
            {
                return;
            }
            
            GamePlay.CustomFixedUpdate();
        }

        public async UniTaskVoid Init()
        {
            await InitAssetManager();
            await InitUIManager();
            await InitInputManager();
            await InitGamePlayManager();
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
        
        private async UniTask InitGamePlayManager()
        {
            GamePlay = new GamePlayManager();
            await GamePlay.Init();
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"Complete Initialize {nameof(GamePlayManager)}");

            OnInitGamePlayManager?.Invoke();
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
