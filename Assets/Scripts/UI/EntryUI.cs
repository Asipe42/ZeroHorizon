using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using Define;
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
        [FoldoutGroup("로그인")] [SerializeField] private Button createAccountButton;
        [FoldoutGroup("로그인")] [SerializeField] private Button loginButton;
        [FoldoutGroup("로그인")] [SerializeField] private Button googleLoginButton;
        
        public EntryUIModel Model { get; private set; }

        private void Awake()
        {
            Bind();
        }

        private void Bind()
        {
            createAccountButton.onClick.AddListener(CreateAccount);
            loginButton.onClick.AddListener(OnLogin);
            googleLoginButton.onClick.AddListener(OnGoogleLogin);
        }
        
        public override void Init()
        {
            base.Init();
            Model = _model as EntryUIModel;
            
            BindEvent();
        }

        public override void Open()
        {
            base.Open();
            
            InitPanel();
            InitProgress();
            InitDescription();
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

        private void ShowProgress(float progress)
        {
            progressBar.value = progress;
            progressText.text = $"{progress * 100:F0}%";
        }
        
        private void ShowDescription(string text)
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
            ShowProgress(0.3f);
            ShowDescription("Initializing Asset Manager");
        }

        private void OnInitUIManager()
        {
            ShowProgress(0.6f);
            ShowDescription("Initializing UI Manager");
        }
        
        private void OnInitAuthManager()
        {
            ShowProgress(1.0f);
            ShowDescription("Initializing Auth Manager");
            
            loginPanel.SetActive(true);
        }

        private void CreateAccount()
        {
            string email = emailInputField.text;
            string password = passwordInputField.text;

            if (IsValidEmail(email) == false)
            {
                GameManager.Instance.UI.OpenUI(ClientEnum.EUIType.ToastMessage, new ToastMessageUIModel()
                {
                    Message = "올바르지 않은 이메일 형식입니다."
                });
                return;
            }

            if (IsValidPassword(password) == false)
            {
                GameManager.Instance.UI.OpenUI(ClientEnum.EUIType.ToastMessage, new ToastMessageUIModel()
                {
                    Message = "올바르지 않은 패스워드 형식입니다."
                });
                return;
            }
            
            GameManager.Instance.Auth.CreateAccountWithEmailAndPassword(email, password).Forget();
        }

        private void OnLogin()
        {
            string email = emailInputField.text;
            string password = passwordInputField.text;

            if (IsValidEmail(email) == false)
            {
                GameManager.Instance.UI.OpenUI(ClientEnum.EUIType.ToastMessage, new ToastMessageUIModel()
                {
                    Message = "올바르지 않은 이메일 형식입니다."
                });
                return;
            }

            if (IsValidPassword(password) == false)
            {
                GameManager.Instance.UI.OpenUI(ClientEnum.EUIType.ToastMessage, new ToastMessageUIModel()
                {
                    Message = "올바르지 않은 패스워드 형식입니다."
                });
                return;
            }
            
            GameManager.Instance.Auth.SignInWithEmailAndPassword(email, password).Forget();
        }

        private void OnGoogleLogin()
        {
            GameManager.Instance.Auth.SignInWithGoogle().Forget();
        }
    }
}