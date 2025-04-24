using Manager;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Actor
{
    public class PlayerCharacter : BaseCharacter
    {
        [FoldoutGroup("PUN2")] [SerializeField] private PhotonView photonView;
        [FoldoutGroup("PUN2")] [SerializeField] private PhotonRigidbodyView photonRigidbodyView;

        [SerializeField] private Rigidbody rigid;

        private Vector2 _input;
        
        public override void Init()
        {
            base.Init();
            
            GameManager.Instance.Input.OnInputHorizontal -= OnInputHorizontal;
            GameManager.Instance.Input.OnInputHorizontal += OnInputHorizontal;

            GameManager.Instance.Input.OnInputVertical -= OnInputVertical;
            GameManager.Instance.Input.OnInputVertical += OnInputVertical;
        }

        public override void CustomUpdate()
        {
            base.CustomUpdate();
            _input.Normalize();
        }

        private void OnInputHorizontal(float value)
        {
            _input.x = value;
        }
        
        private void OnInputVertical(float value)
        {
            _input.y = value;
        }
    }
}