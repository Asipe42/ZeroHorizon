using Manager;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyUI : BaseUI
    {
        [FoldoutGroup("방 생성")] [SerializeField] private TMP_InputField createRoomInputField;
        [FoldoutGroup("방 생성")] [SerializeField] private Button createRoomButton;
        
        [FoldoutGroup("방 참가")] [SerializeField] private TMP_InputField joinRoomInputField;
        [FoldoutGroup("방 참가")] [SerializeField] private Button joinRoomButton;

        private void Awake()
        {
            Bind();
        }

        private void Bind()
        {
            createRoomButton.onClick.AddListener(OnCreateRoom);
            joinRoomButton.onClick.AddListener(OnJoinRoom);
        }

        private void OnCreateRoom()
        {
            string roomName = createRoomInputField.text;
            RoomManager.Instance.CreateRoom(roomName);
        }

        private void OnJoinRoom()
        {
            string roomName = joinRoomInputField.text;
            RoomManager.Instance.JoinRoom(roomName);
        }
    }
}