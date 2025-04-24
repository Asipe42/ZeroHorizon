using Photon.Pun;
using UnityEngine;

namespace Manager
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public void ConnectMaster()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.CreateRoom(roomName);
        }

        public void CreateRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            
            Debug.Log("OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            
            Debug.Log("OnJoinedLobby");
        }
    }
}