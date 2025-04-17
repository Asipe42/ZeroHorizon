using Config;
using Define;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Scene에 대한 제어 및 관리를 담당한다.
    /// </summary>
    public class BaseSceneController : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public Light MainLight { get; private set; }
        [field: SerializeField] public BaseSceneConfig Config { get; private set; }

        public virtual ClientEnum.ESceneType Type { get; protected set; }
        
        public virtual void Init()
        {
            OnInit();
        }

        public virtual void CleanUp()
        {
            OnCleanUp();
        }

        protected virtual void OnInit()
        {
            // GameManager.Instance.UI.OpenUI(ClientEnum.EUIType.Login);
        }

        protected virtual void OnCleanUp()
        {
            
        }
    }
}