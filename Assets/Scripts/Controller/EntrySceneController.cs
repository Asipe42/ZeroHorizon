using Define;
using Manager;

namespace Controller
{
    /// <summary>
    /// 프로그램 진입점
    /// </summary>
    public class EntrySceneController : BaseSceneController
    {
        public override ClientEnum.ESceneType Type => ClientEnum.ESceneType.Entry;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            // 초기화 이후 [Auth]로 이동한다.
            GameManager.Instance.LoadScene(ClientEnum.ESceneType.Auth);
        }
    }
}