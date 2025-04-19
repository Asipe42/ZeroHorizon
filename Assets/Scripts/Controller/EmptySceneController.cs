using Define;
using Manager;

namespace Controller
{
    /// <summary>
    /// 프로그램 진입점
    /// </summary>
    public class EmptySceneController : BaseSceneController
    {
        public override ClientEnum.ESceneType Type => ClientEnum.ESceneType.Empty;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            GameManager.Instance.LoadScene(ClientEnum.ESceneType.Entry);
        }
    }
}