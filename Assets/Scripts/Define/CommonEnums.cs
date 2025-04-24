namespace Define
{
    public enum SceneType
    {
        None,
        Empty,
        Entry,
        Main,
    }

    public enum UIType
    {
        None,
        Entry,
        ToastMessage,
        Lobby,
        Loading
    }

    public enum AuthState
    {
        None,
        HasUID,
        HasUserInfo
    }
}