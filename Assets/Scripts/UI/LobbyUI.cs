using Define;
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
        
        public override void Open()
        {
            base.Open();
            
            RoomManager.Instance.OnJoinedRoomEvent -= OnJoinedRoomEventEvent;
            RoomManager.Instance.OnJoinedRoomEvent += OnJoinedRoomEventEvent;
        }

        private void OnCreateRoom()
        {
            GameManager.Instance.UI.OpenUI(UIType.Loading, new LoadingUIModel());
            
            string roomName = createRoomInputField.text;
            RoomManager.Instance.CreateRoom(roomName);
        }

        private void OnJoinRoom()
        {
            GameManager.Instance.UI.OpenUI(UIType.Loading, new LoadingUIModel());
            
            string roomName = joinRoomInputField.text;
            RoomManager.Instance.JoinRoom(roomName);
        }

        private void OnJoinedRoomEventEvent()
        {
            GameManager.Instance.UI.CloseUI(UIType.Loading);
        }
    }
}