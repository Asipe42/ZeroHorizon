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
                DependencyStatus dependencyStatus = task.Result;
                if (dependencyStatus != DependencyStatus.Available)
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    return;
                }
                
                _auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth initialized.");
            });
        }

        public async UniTask SignInWithEmail(string email, string password)
        {
            await _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {   
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError($"Sign in failed: {task.Exception}");
                    return;
                }

                _user = task.Result.User;
                Debug.Log($"User signed in successfully: {_user.Email}");
            });
        }

        public async UniTask SignInWithGoogle()
        {
            
        }
    }
}