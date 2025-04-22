using System;

namespace Define
{
    [Serializable]
    public class AccountData
    {
        
    }

    [Serializable]
    public class EmailAccountData : AccountData
    {
        public string email;
        public string password;
    }

    [Serializable]
    public class GoogleAccountData : AccountData
    {
        public string refreshToken;
    }
}