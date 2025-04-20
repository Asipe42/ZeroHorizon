using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

namespace Manager
{
    public class AuthManager
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        
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

        public async UniTask SignInWithEmail(string email, string password)
        {
            try
            {
                // 계정이 없을 경우 자동 생성 후 로그인 시도
                AuthResult userCredential = await _auth.SignInWithEmailAndPasswordAsync(email, password);
                _user = userCredential.User;
                
                Debug.Log($"User signed in successfully: {_user.Email}");
            }
            catch (FirebaseException ex) when (ex.ErrorCode == (int)AuthError.UserNotFound)
            {
                Debug.Log("User not found. Trying to create a new user...");

                try
                {
                    AuthResult newUserCredential = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                    _user = newUserCredential.User;
                    Debug.Log($"User created and signed in: {_user.Email}");
                }
                catch (Exception createEx)
                {
                    Debug.LogError($"User creation failed: {createEx}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Sign in failed: {ex}");
            }
        }

        public async UniTask SignInWithGoogle()
        {
            
        }
    }
}