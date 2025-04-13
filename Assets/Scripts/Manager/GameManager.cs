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
        
        protected override void Awake()
        {
            base.Awake();
            
            Init();
        }

        private void Init()
        {
            InitEvent();
            InitSubManager();
        }

        private void InitEvent()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void InitSubManager()
        {
            UI = new UIManager();
        }
    }
}
