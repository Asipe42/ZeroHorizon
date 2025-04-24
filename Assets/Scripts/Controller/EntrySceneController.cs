using Define;
using Manager;
using UI;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// 로그인 씬
    /// </summary>
    public class EntrySceneController : BaseSceneController
    {
        [SerializeField] private EntryUI entryUI;
        
        public override SceneType Type => SceneType.Entry;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            /*
             * 초기화
             *  1. 게임 매니저 초기화
             *      - 초기화 과정을 표시 (EntryUI)
             *  2. 계정 정보를 가져온다.
             *  3. 계정 정보를 가져오지 못한 경우 신규 계정 UI 표시
             */
            
            OpenEntryUI();
            GameManager.Instance.Init().Forget();
        }

        private void OpenEntryUI()
        {
            EntryUIModel model = new();
            model.Init();
            
            entryUI.BindModel(model);
            entryUI.Init();
            entryUI.Open();
        }
    }
}