using Define;

namespace UI
{
    public class BaseUIModel
    {
        public ClientEnum.EUIType UIType { get; protected set; }

        public virtual void Init()
        {
            
        }

        public virtual void CleanUp()
        {
            
        }
    }
}