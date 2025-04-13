using Config;
using Define;
using Handler;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Scene에 대한 제어 및 관리를 담당한다.
    /// </summary>
    public class BaseSceneController : MonoBehaviour
    {
        [SerializeField] protected BaseSceneConfig config;

        public BaseSceneHandler SceneHandler { get; private set; }
        public virtual ClientEnum.ESceneType Type { get; protected set; }
        
        public virtual void Init()
        {
            
        }

        public virtual void CleanUp()
        {
            
        }
    }
}