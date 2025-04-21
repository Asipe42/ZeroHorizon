using System.Text.RegularExpressions;
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

            InitPanel();
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

        private void InitPanel()
        {
            loadingPanel.SetActive(true);
            loginPanel.SetActive(false);
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

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            /*
             * 정규식
             *  - [^@\s]+ : @나 공백이 없는 문자열로 시작
             *  - @ : 반드시 @ 포함
             *  - [^@\s]+ : 도메인 이름
             *  - \. : 점 포함
             *  - [^@\s]+ : 도메인 끝 (예: .com, .net 등)
             */
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            
            /*
             * 정규식
             *  - [a-zA-Z0-9]: 영문 대소문자와 숫자 허용
             *  - {6,}: 6자 이상
             */
            string pattern = @"^[a-zA-Z0-9]{6,}$";
            return Regex.IsMatch(password, pattern);
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
            
            loginPanel.SetActive(true);
        }

        private void OnLogin()
        {
            string email = emailInputField.text;
            string password = passwordInputField.text;

            if (IsValidEmail(email) == false)
            {
                Debug.LogError($"올바르지 않은 이메일 형식입니다.");
                return;
            }

            if (IsValidPassword(password) == false)
            {
                Debug.LogError($"올바르지 않은 패스워드 형식입니다.");
                return;
            }
            
            GameManager.Instance.Auth.SignInWithEmail(email, password).Forget();
        }

        private void OnGoogleLogin()
        {
            GameManager.Instance.Auth.SignInWithGoogle().Forget();
        }
    }
}