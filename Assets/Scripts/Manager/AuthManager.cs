using System;
using Cysharp.Threading.Tasks;
using Define;
using Firebase;
using Firebase.Auth;
using UnityEngine;

namespace Manager
{
    public class AuthManager
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;

        private const string ClientID = "985978133149-mr8pmvd96c3j6eoi3bkbh47bd4a31of9.apps.googleusercontent.com";
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

        public async UniTask SignInWithEmailAndPassword(string email, string password, Action successCallback = null, Action<ClientEnum.EAuthError> failedCallback = null)
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

        public async UniTask SignInWithGoogle()
        {
            // 인증 받기
            string authUrl = string.Join("&", new[]
            {
                "https://accounts.google.com/o/oauth2/v2/auth",
                $"client_id={ClientID}",
                $"redirect_uri={Uri.EscapeDataString(RedirectUri)}",
                "response_type=code",
                $"scope={Uri.EscapeDataString(Scope)}",
                "access_type=offline"
            });
            Application.OpenURL(authUrl);
        }
        
        public async UniTask CreateAccountWithEmailAndPassword(string email, string password, Action successCallback = null, Action<ClientEnum.EAuthError> failedCallback = null)
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
    }
}