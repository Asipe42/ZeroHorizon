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
    }
}