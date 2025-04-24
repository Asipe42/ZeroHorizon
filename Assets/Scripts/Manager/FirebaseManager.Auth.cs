using System;
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
    public partial class FirebaseManager
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private string _uid;

        private const string ClientID = "985978133149-mr8pmvd96c3j6eoi3bkbh47bd4a31of9.apps.googleusercontent.com";
        private const string ClientSecret = "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";
        private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        private const string Scope = "email profile openid";
        
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
    }
}