using System.Collections.Generic;
using Controller;
using Define;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public partial class GameManager
    {
        public ClientEnum.ESceneType CurrentSceneType { get; private set; }
        public BaseSceneController CurrentSceneController { get; private set; }

        private Dictionary<ClientEnum.ESceneType, LoadSceneParameters> _sceneParameters = new()
        {
            { ClientEnum.ESceneType.Entry, new LoadSceneParameters(LoadSceneMode.Single) },
            { ClientEnum.ESceneType.Auth, new LoadSceneParameters(LoadSceneMode.Single) },
            { ClientEnum.ESceneType.Main, new LoadSceneParameters(LoadSceneMode.Single) },
        };
        
        public void LoadScene(ClientEnum.ESceneType sceneType)
        {
            if (CurrentSceneType == sceneType)
            {
                Debug.Log("Already in the same scene.");
            }
            
            CurrentSceneController?.CleanUp();
            
            AsyncOperation operation = SceneManager.LoadSceneAsync($"{sceneType}", _sceneParameters[sceneType]);
            if (operation == null)
            {
                Debug.LogError($"Failed to load scene {sceneType}");
                return;
            }

            operation.completed -= OnCompletedLoadScene;
            operation.completed += OnCompletedLoadScene;
        }
        
        private bool TryFindSceneController(Scene scene, out BaseSceneController controller)
        {
            controller = null;
            
            GameObject[] goArray = scene.GetRootGameObjects();
            foreach (var each in goArray)
            {
                if (each.gameObject.TryGetComponent(out controller))
                {
                    return true;
                }
            }

            return false;
        }
        
        private void OnCompletedLoadScene(AsyncOperation operation)
        {
            Debug.Log("Scene loading completed.");
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            Debug.Log("Scene unloaded: " + scene.name);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (TryFindSceneController(scene, out var controller) == false)
            {
                Debug.LogError($"SceneController not found in {scene.name}");
                return;
            }

            CurrentSceneController = controller;
            CurrentSceneController.Init();
            CurrentSceneType = CurrentSceneController.Type;
            
            Debug.Log("Scene loaded: " + CurrentSceneType);
        }
    }
}