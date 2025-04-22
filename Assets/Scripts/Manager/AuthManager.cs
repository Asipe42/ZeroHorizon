using System;
using System.Text;
using Define;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Manager
{
    public class AuthManager
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;

        private const string ClientID = "985978133149-mr8pmvd96c3j6eoi3bkbh47bd4a31of9.apps.googleusercontent.com";
        private const string ClientSecret = "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";
        private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        private const string Scope = "email profile openid";
        
        public async UniTask Init()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                    return;
                }

                // google.services.json 파일 대신 수동 초기화
                AppOptions appOptions = new AppOptions()
                {
                    ApiKey = "AIzaSyBNrtipIiWxyUzID_45N03R4JbuRKUO43s",
                    AppId = "1:382186013137:web:3a8c36546268779fd820f0",
                    ProjectId = "zerohorizon-1f654",
                };
                
                FirebaseApp app = FirebaseApp.Create(appOptions);
                _auth = FirebaseAuth.DefaultInstance;
                
                Debug.Log("Firebase Auth initialized.");
            });
        }

        public async UniTask SignInWithEmail(string email, string password, Action successCallback = null, Action<ClientEnum.EAuthError> failedCallback = null)
        {
            try
            {
                AuthResult userCredential = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                _user = userCredential.User;
                
                successCallback?.Invoke();
                Debug.Log($"User signed in successfully: {_user.Email}");
            }
            catch (FirebaseException ex)
            {
                ClientEnum.EAuthError errorCode = (ClientEnum.EAuthError)ex.ErrorCode;
                failedCallback?.Invoke(errorCode);
                Debug.LogError($"Sign in failed: {ex}");
            }
        }
        
        public async UniTask SignInWithGoogle(string authorizationCode)
        {
            using var request = new UnityWebRequest("https://oauth2.googleapis.com/token", "POST");
            string body = $"code={UnityWebRequest.EscapeURL(authorizationCode)}&" +
                          $"client_id={UnityWebRequest.EscapeURL(ClientID)}&" +
                          $"client_secret={UnityWebRequest.EscapeURL(ClientSecret)}&" +
                          $"redirect_uri={UnityWebRequest.EscapeURL(RedirectUri)}&" +
                          $"grant_type=authorization_code";
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log($"Token Response: {json}");

                TokenResponse tokenData = JsonUtility.FromJson<TokenResponse>(json);
                Credential credential = GoogleAuthProvider.GetCredential(tokenData.id_token, null);
                
                try
                {
                    FirebaseUser user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
                    Debug.Log("User signed in: " + user.DisplayName);
                }
                catch (FirebaseException e)
                {
                    Debug.LogError("Error signing in with Google: " + e.Message);
                }
            }
            else
            {
                Debug.LogError($"Token request failed: {request.error}");
            }
        }
        
        public async UniTask CreateAccountWithEmail(string email, string password, Action successCallback = null, Action<ClientEnum.EAuthError> failedCallback = null)
        {
            try
            {
                AuthResult newUserCredential = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                _user = newUserCredential.User;
                
                successCallback?.Invoke();
                Debug.Log($"User created and signed in: {_user.Email}");
            }
            catch (FirebaseException ex)
            {
                ClientEnum.EAuthError errorCode = (ClientEnum.EAuthError)ex.ErrorCode;
                failedCallback?.Invoke(errorCode);
                Debug.LogError($"User create failed: {ex}");
                throw;
            }
        }
        
        public void OpenGoogleAuthURL()
        {
            string authUrl = "https://accounts.google.com/o/oauth2/v2/auth?" + string.Join
            (
                "&", 
                $"client_id={ClientID}", 
                $"redirect_uri={Uri.EscapeDataString(RedirectUri)}", 
                "response_type=code", 
                $"scope={Uri.EscapeDataString(Scope)}", 
                "access_type=offline"
            );
            Application.OpenURL(authUrl);
        }
    }
}