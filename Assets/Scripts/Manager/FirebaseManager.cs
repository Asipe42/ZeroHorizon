using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Define;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Handler;
using UnityEngine;
using UnityEngine.Networking;

namespace Manager
{
    public class FirebaseManager
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private FirebaseFirestore _firestore;
        private string _uid;
        private string _nickname;
        
        public string UID => _uid;
        public string Nickname => _nickname;
        
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
                _firestore = FirebaseFirestore.DefaultInstance;
                
                Debug.Log("Firebase Auth initialized.");
            });

            if (LocalDBHandler.TryGetUID(out _uid))
            {
                Debug.Log("Login record found.");
            }
            else
            {
                Debug.Log("No login record found.");
            }
        }
        
        public async UniTask SignInWithEmail(string email, string password, Action successCallback = null, Action failedCallback = null)
        {
            try
            {
                AuthResult authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                _user = authResult.User;
                LocalDBHandler.WriteUID(_user.UserId);
                
                successCallback?.Invoke();
                Debug.Log($"User signed in: {_user.UserId}");
            }
            catch (FirebaseException ex)
            {
                failedCallback?.Invoke();
                Debug.LogError($"Sign in failed: {ex.Message}");
            }
        }
        
        public async UniTask SignInWithGoogle(string code, Action successCallback = null, Action failedCallback = null)
        {
            using var request = new UnityWebRequest("https://oauth2.googleapis.com/token", "POST");
            string body = $"code={UnityWebRequest.EscapeURL(code)}&" +
                          $"client_id={UnityWebRequest.EscapeURL(ClientID)}&" +
                          $"client_secret={UnityWebRequest.EscapeURL(ClientSecret)}&" +
                          $"redirect_uri={UnityWebRequest.EscapeURL(RedirectUri)}&" +
                          "grant_type=authorization_code";
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
                    _user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
                    LocalDBHandler.WriteUID(_user.UserId);
                    
                    successCallback?.Invoke();
                    Debug.Log($"User signed in: {_user.UserId}");
                }
                catch (FirebaseException ex)
                {
                    failedCallback?.Invoke();
                    Debug.LogError($"Sign in failed: {ex.Message}");
                }
            }
            else
            {
                failedCallback?.Invoke();
                Debug.LogError($"Token request failed: {request.error}");
            }
        }
        
        public async UniTask CreateAccountWithEmail(string email, string password, Action successCallback = null, Action failedCallback = null)
        {
            try
            {
                AuthResult authResult = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                _user = authResult.User;
                LocalDBHandler.WriteUID(_user.UserId);
                
                successCallback?.Invoke();
                Debug.Log($"User created and signed in: {_user.Email}");
            }
            catch (FirebaseException ex)
            {
                failedCallback?.Invoke();
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
        

        // UserInfo를 생성한다.
        // UserInfo를 확인한다.
        
        public void CreateUserInfo(string nickname, Action successCallback = null, Action failedCallback = null)
        {
            DocumentReference uidRef = _firestore.Collection("USERS").Document(_uid);
            uidRef.SetAsync
            (
                documentData: new Dictionary<string, object>()
                {
                    { "NICKNAME", nickname },
                    { "CREATED_AT", FieldValue.ServerTimestamp }
                }
            ).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted == false)
                {
                    failedCallback?.Invoke();
                    return;
                }

                _nickname = nickname;
                
                Debug.Log("Success creating user info");
                successCallback?.Invoke();
            } );
        }

        public async UniTask<bool> IsExistNickname(string nickname)
        {
            CollectionReference usersRef = _firestore.Collection("USERS");
            QuerySnapshot snapshot = await usersRef.WhereEqualTo("NICKNAME", nickname).GetSnapshotAsync();
            return snapshot.Count > 0;
        }

        public async UniTask<bool> IsExistUserInfo(string uid)
        {
            DocumentReference uidRef = _firestore.Collection("USERS").Document(uid);
            DocumentSnapshot snapshot = await uidRef.GetSnapshotAsync();
            return snapshot.Exists;
        }
    }
}