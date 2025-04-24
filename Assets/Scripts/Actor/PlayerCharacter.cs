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
    }
}