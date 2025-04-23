using Define;
using Manager;

namespace Controller
{
    /// <summary>
    /// 프로그램 진입점
    /// </summary>
    public class EmptySceneController : BaseSceneController
    {
        public override SceneType Type => SceneType.Empty;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            GameManager.Instance.LoadScene(SceneType.Entry);
        }
    }
}