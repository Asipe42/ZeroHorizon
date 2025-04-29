using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Manager
{
    public class InputManager
    {
        public event Action<float> OnInputHorizontal;
        public event Action<float> OnInputVertical;
        
        public async UniTask Init()
        {
            
        }
        
        public void CustomUpdate()
        {
            OnInputHorizontal?.Invoke(Input.GetAxisRaw("Horizontal"));
            OnInputVertical?.Invoke(Input.GetAxisRaw("Vertical"));
        }
    }
}