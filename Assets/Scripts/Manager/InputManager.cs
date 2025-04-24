using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Manager
{
    public class InputManager
    {
        public event Action<float> OnInputHorizontal;
        public event Action<float> OnInputVertical;

        private bool _isInit;
        
        public async UniTask Init()
        {
            _isInit = true;
        }
        
        public void CustomUpdate()
        {
            if (!_isInit)
            {
                return;
            }
            
            OnInputHorizontal?.Invoke(Input.GetAxisRaw("Horizontal"));
            OnInputVertical?.Invoke(Input.GetAxisRaw("Vertical"));
        }
    }
}