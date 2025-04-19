using Cysharp.Threading.Tasks;
using Manager;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EntryUI : BaseUI
    {
        [FoldoutGroup("로딩")] [SerializeField] private GameObject loadingPanel;
        [FoldoutGroup("로딩")] [SerializeField] private Slider progressBar;
        [FoldoutGroup("로딩")] [SerializeField] private TextMeshProUGUI progressText;
        [FoldoutGroup("로딩")] [SerializeField] private TextMeshProUGUI descriptionText;

        [FoldoutGroup("로그인")] [SerializeField] private GameObject loginPanel;
        [FoldoutGroup("로그인")] [SerializeField] private TMP_InputField emailInputField;
        [FoldoutGroup("로그인")] [SerializeField] private TMP_InputField passwordInputField;
        [FoldoutGroup("로그인")] [SerializeField] private Button loginButton;
        [FoldoutGroup("로그인")] [SerializeField] private Button googleLoginButton;
        
        public EntryUIModel Model { get; private set; }

        private void Awake()
        {
            Bind();
        }

        private void Bind()
        {
            loginButton.onClick.AddListener(OnLogin);
            googleLoginButton.onClick.AddListener(OnGoogleLogin);
        }

        public override void Open()
        {
            base.Open();
            
            InitProgress();
            InitDescription();
        }

        public override void Init()
        {
            base.Init();
            Model = _model as EntryUIModel;
            
            BindEvent();
        }
        
        private void BindEvent()
        {
            GameManager.Instance.OnInitAssetManager -= OnInitAssetManager;
            GameManager.Instance.OnInitAssetManager += OnInitAssetManager;
            
            GameManager.Instance.OnInitUIManager -= OnInitUIManager;
            GameManager.Instance.OnInitUIManager += OnInitUIManager;
            
            GameManager.Instance.OnInitAuthManager -= OnInitAuthManager;
            GameManager.Instance.OnInitAuthManager += OnInitAuthManager;
        }

        private void InitProgress()
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0;
            progressText.text = string.Empty;
        }
        
        private void InitDescription()
        {
            descriptionText.text = string.Empty;
        }

        private void SetProgress(float progress)
        {
            progressBar.value = progress;
            progressText.text = $"{progress * 100:F0}%";
        }
        
        private void SetDescription(string text)
        {
            descriptionText.text = text;
        }
        
        private void OnInitAssetManager()
        {
            SetProgress(0.3f);
            SetDescription("Initializing Asset Manager");
        }

        private void OnInitUIManager()
        {
            SetProgress(0.6f);
            SetDescription("Initializing UI Manager");
        }
        
        private void OnInitAuthManager()
        {
            SetProgress(1.0f);
            SetDescription("Initializing Auth Manager");
        }

        private void OnLogin()
        {
            string email = emailInputField.text;
            string password = passwordInputField.text;
            GameManager.Instance.Auth.SignInWithEmail(email, password).Forget();
        }

        private void OnGoogleLogin()
        {
            GameManager.Instance.Auth.SignInWithGoogle().Forget();
        }
    }
}