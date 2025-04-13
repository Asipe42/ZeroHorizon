using Config;
using Define;
using Handler;
using UnityEngine;
using Utility;

namespace Controller
{
    /// <summary>
    /// Scene에 대한 제어 및 관리를 담당한다.
    /// </summary>
    public class BaseSceneController : MonoSingleton<BaseSceneController>
    {
        [SerializeField] protected BaseSceneConfig config;

        public BaseSceneHandler SceneHandler { get; private set; }
        public virtual ClientEnum.ESceneType SceneType { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            
        }
    }
}