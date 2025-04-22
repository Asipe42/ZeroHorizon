using Define;

namespace UI
{
    public class BaseUIModel
    {
        public EUIType UIType { get; protected set; }

        public virtual void Init()
        {
            
        }

        public virtual void CleanUp()
        {
            
        }
    }
}