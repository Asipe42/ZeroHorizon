using TMPro;
using UnityEngine;

namespace UI
{
    public class ToastMessageUI : BaseUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Animator animator;

        public ToastMessageUIModel Model { get; private set; }

        public override void Open()
        {
            base.Open();
            Model = _model as ToastMessageUIModel;

            ShowMessage();
            ShowAnimation();
        }

        private void ShowMessage()
        {
            messageText.text = Model.Message;
        }

        private void ShowAnimation()
        {
            animator.Play("ToastMessageUI_Show", 0, 0f);
        }

        #region Animation Callback

        public void HideCallback()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}