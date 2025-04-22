using System;

namespace Define
{
    [Serializable]
    public class TokenResponse
    {
        public string access_token;
        public string refresh_token;
        public string expires_in;
        public string token_type;
        public string id_token;
    }
}