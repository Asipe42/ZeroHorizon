using System;
using Photon.Pun;
using UnityEngine;

namespace Manager
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public static RoomManager Instance { get; private set; }
        
        public event Action OnConnectedLobby;
        public event Action OnCreateRoom;
        public event Action OnJoinRoom;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
            
            OnConnectedLobby?.Invoke();
            Debug.Log("OnJoinedLobby");
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            
            OnCreateRoom?.Invoke();
            Debug.Log("OnCreatedRoom");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            
            OnJoinRoom?.Invoke();
            Debug.Log("OnJoinRoom");
        }
    }
}