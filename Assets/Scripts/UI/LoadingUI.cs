namespace UI
{
    public class LoadingUI : BaseUI
    {
        public LoadingUIModel Model { get; private set; }

        public override void Open()
        {
            base.Open();
            Model = _model as LoadingUIModel;
        }
    }
}