using Controller;
using Define;
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
        
        [SerializeField] private float speed;
        [SerializeField] private float maxVelocityChange;

        [SerializeField] private Rigidbody rigid;
        [SerializeField] private Animator animator;

        private Vector2 _input;
        
        private static readonly int Velocity = Animator.StringToHash("Velocity");

        public override void Init()
        {
            base.Init();
            
            GameManager.Instance.Input.OnInputHorizontal -= OnInputHorizontal;
            GameManager.Instance.Input.OnInputHorizontal += OnInputHorizontal;

            GameManager.Instance.Input.OnInputVertical -= OnInputVertical;
            GameManager.Instance.Input.OnInputVertical += OnInputVertical;
            
            GameManager.Instance.GamePlay.RegisterActor(this);
        }

        public override void CustomUpdate()
        {
            base.CustomUpdate();

            UpdateInput();
            UpdateAnimation();
            UpdateRotation();
        }

        private void UpdateInput()
        {
            _input.Normalize();
        }

        private void UpdateAnimation()
        {
            animator.SetFloat(Velocity, rigid.linearVelocity.normalized.magnitude);
        }

        private void UpdateRotation()
        {
            Ray ray = GameManager.Instance.CurrentSceneController.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, Layer.Ground))
            {
                Vector3 targetPos = hit.point;
                Vector3 direction = (targetPos - transform.position).normalized;
                direction.y = 0f;

                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                }
            }
        }

        public override void CustomFixedUpdate()
        {
            base.CustomFixedUpdate();

            FixedUpdateRigidbody();
        }

        private void FixedUpdateRigidbody()
        {
            rigid.AddForce(CalcMovement(), ForceMode.VelocityChange);
        }

        private Vector3 CalcMovement()
        {
            Vector3 targetVelocity = new Vector3(_input.x, 0, _input.y);
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;
            
            Vector3 nowVelocity = rigid.linearVelocity;
            if (_input.magnitude > 0.5f)
            {
                Vector3 changedVelocity = targetVelocity - nowVelocity;
                changedVelocity.x = Mathf.Clamp(changedVelocity.x, -maxVelocityChange, maxVelocityChange);
                changedVelocity.z = Mathf.Clamp(changedVelocity.z, -maxVelocityChange, maxVelocityChange);
                changedVelocity.y = 0;

                return changedVelocity;
            }
            else
            {
                return new Vector3();
            }
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