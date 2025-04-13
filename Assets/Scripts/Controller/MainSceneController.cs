using Define;

namespace Controller
{
    public class MainSceneController : BaseSceneController
    {
        public override ClientEnum.ESceneType Type => ClientEnum.ESceneType.Main;

        public override void Init()
        {
            base.Init();
        }

        public override void CleanUp()
        {
            base.CleanUp();
        }
    }
}