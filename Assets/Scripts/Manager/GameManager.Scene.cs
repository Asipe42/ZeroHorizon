using System.Collections.Generic;
using Controller;
using Define;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public partial class GameManager
    {
        public SceneType CurrentSceneType { get; private set; }
        public BaseSceneController CurrentSceneController { get; private set; }

        private readonly Dictionary<SceneType, LoadSceneParameters> _sceneParameters = new()
        {
            { SceneType.Empty, new LoadSceneParameters(LoadSceneMode.Single) },
            { SceneType.Entry, new LoadSceneParameters(LoadSceneMode.Single) },
            { SceneType.Main, new LoadSceneParameters(LoadSceneMode.Single) },
        };
        
        public void LoadScene(SceneType sceneType)
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