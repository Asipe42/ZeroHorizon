using Define;
using Manager;

namespace Controller
{
    public class EntrySceneController : BaseSceneController
    {
        public override ClientEnum.ESceneType Type => ClientEnum.ESceneType.Entry;

        public override void Init()
        {
            base.Init();
        }

        public override void CleanUp()
        {
            base.CleanUp();
        }

        private void Start()
        {
            // 임시 코드
            GameManager.Instance.LoadScene(ClientEnum.ESceneType.Auth);
        }
    }
}