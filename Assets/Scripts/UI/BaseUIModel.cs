using Define;

namespace UI
{
    public class BaseUIModel
    {
        public ClientEnum.EUIType UIType { get; protected set; }
        
        public virtual void Open()
        {
            OnOpen();
        }

        public virtual void Close()
        {
            OnClose();
        }

        protected virtual void OnOpen()
        {
            
        }

        protected virtual void OnClose()
        {
            
        }
    }
}