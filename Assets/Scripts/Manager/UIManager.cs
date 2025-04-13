using System.Collections.Generic;
using Define;
using UI;
using UnityEngine;

namespace Manager
{
    public class UIManager
    {
        private Dictionary<ClientEnum.EUIType, UIBinding> _uiBindings = new();
        
        public void OpenUI(ClientEnum.EUIType uiType)
        {
            if (TryFindModel(uiType, out var model) == false)
            {
                
            }
            
            model.Open();
            Debug.Log($"Showing UI: {uiType}");
        }
        
        public void CloseUI(ClientEnum.EUIType uiType)
        {
            if (TryFindModel(uiType, out var model) == false)
            {
                Debug.LogWarning("UI not found: " + uiType);
                return;
            }
            
            model.Close();
            Debug.Log($"Closing UI: {uiType}");
        }
        
        public bool TryFindModel(ClientEnum.EUIType uiType, out BaseUIModel model)
        {
            model = null;
            
            if (_uiBindings.TryGetValue(uiType, out var result) == false)
            {
                return false;
            }
            
            model = result.UIModel;
            return model != null;
        }
    }
}