using Define;
using UnityEngine;

namespace Manager
{
    public class UIManager
    {
        public void OpenUI(ClientEnum.EUIType uiType)
        {
            Debug.Log($"Showing UI: {uiType}");
        }
        
        public void CloseUI(ClientEnum.EUIType uiType)
        {
            Debug.Log($"Closing UI: {uiType}");
        }
    }
}