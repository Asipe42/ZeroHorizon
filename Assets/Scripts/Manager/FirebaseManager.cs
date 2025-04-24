using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Define;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Handler;
using UnityEngine;
using UnityEngine.Networking;

namespace Manager
{
    public class FirebaseManager
    {
        private FirebaseAuth _auth;
        private FirebaseFirestore _firestore;
        
        private FirebaseUser _user;
        private string _uid;
        private string _nickname;
        private AuthState _authState;
        
        public string UID => _uid;
        public string Nickname => _nickname;
        public AuthState AuthState => _authState;
        
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

            await HandleAuthFlow();
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
        
        public async UniTask SignInWithEmailAndPassword(string email, string password, Action successCallback = null, Action failedCallback = null)
        {
            try
            {
                AuthResult result = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                SetFirebaseUser(result.User);
                
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
                    FirebaseUser user = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
                    SetFirebaseUser(user);
                    
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
        
        public async UniTask CreateUserAccount(string email, string password, Action successCallback = null, Action failedCallback = null)
        {
            try
            {
                await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                
                successCallback?.Invoke();
                Debug.Log($"User created and signed in: {email}");
            }
            catch (FirebaseException ex)
            {
                failedCallback?.Invoke();
                Debug.LogError($"User create failed: {ex}");
            }
        }
        
        public async UniTaskVoid CreateUserInfo(string uid, string nickname, Action successCallback = null, Action failedCallback = null)
        {
            if (await IsExistsUserInfo(uid) || await IsExistsNickname(nickname))
            {
                failedCallback?.Invoke();
                return;
            }
            
            try
            {
                Dictionary<string, object> userData = new Dictionary<string, object>
                {
                    { "NICKNAME", nickname },
                    { "CREATED_AT", FieldValue.ServerTimestamp }
                };

                DocumentReference uidRef = _firestore.Collection("USERS").Document(uid);
                await uidRef.SetAsync(userData);
                SetNickname(nickname);

                successCallback?.Invoke();
                Debug.Log("Success creating user info");
            }
            catch (Exception e)
            {
                failedCallback?.Invoke();
                Debug.LogError($"CreateUserInfo failed: {e.Message}");
            }
        }

        private async UniTask<bool> IsExistsNickname(string nickname)
        {
            CollectionReference usersRef = _firestore.Collection("USERS");
            QuerySnapshot snapshot = await usersRef.WhereEqualTo("NICKNAME", nickname).GetSnapshotAsync();
            return snapshot.Count > 0;
        }

        private async UniTask<bool> IsExistsUserInfo(string uid)
        {
            DocumentReference uidRef = _firestore.Collection("USERS").Document(uid);
            DocumentSnapshot snapshot = await uidRef.GetSnapshotAsync();
            return snapshot.Exists;
        }

        private void SetFirebaseUser(FirebaseUser user)
        {
            _user = user;
            _uid = _user.UserId;
            LocalDBHandler.WriteUID(_uid);
        }
        
        private void SetNickname(string nickname)
        {
            _nickname = nickname;
            LocalDBHandler.WriteNickname(_nickname);
        }
        
        private async UniTask HandleAuthFlow()
        {
            /*
             * 시나리오
             *  A. UID가 존재하지 않는 경우
             *      - 아무것도 하지 않는다.
             *  B. UID는 존재하고 유저 정보가 존재하지 않는 경우
             *      - 유저 정보 생성 단계로 넘어간다.
             *  C. UID, 유저 정보 모두 존재하는 경우
             *      - 씬 전환 단계로 넘어간다.
             */
            
            // Case A
            if (LocalDBHandler.TryGetUID(out _uid) == false)
            {
                Debug.Log("No login record found.");
                return;
            }
            
            Debug.Log("Login record found.");
            
            // Case B
            if (await IsExistsUserInfo(_uid) == false)
            {
                HandleUserInfoFlow();
                return;
            }
            
            // Case C
            HandleMainSceneFlow();
        }

        private void HandleUserInfoFlow()
        {
            _authState = AuthState.HasUID;
        }

        private void HandleMainSceneFlow()
        {
            _authState = AuthState.HasUserInfo;
            LocalDBHandler.TryGetNickname(out _nickname);
        }
    }
}