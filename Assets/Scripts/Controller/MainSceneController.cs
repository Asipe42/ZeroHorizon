using Define;
using Manager;
using UI;

namespace Controller
{
    public class MainSceneController : BaseSceneController
    {
        public override SceneType Type => SceneType.Main;

        public override void Init()
        {
            base.Init();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            GameManager.Instance.UI.OpenUI(UIType.Lobby, new LobbyUIModel());
            RoomManager.Instance.ConnectMaster();
        }
    }
}