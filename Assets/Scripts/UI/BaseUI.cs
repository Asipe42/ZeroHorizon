using UnityEngine;

namespace UI
{
    public class BaseUI : MonoBehaviour
    {
        protected BaseUIModel _model;
        
        public virtual void Init()
        {
            gameObject.SetActive(false);
            DontDestroyOnLoad(this);
        }
        
        public virtual void BindModel(BaseUIModel model)
        {
            _model = model;
        }

        public virtual void CleanUp()
        {
            _model.CleanUp();
            _model = null;
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}