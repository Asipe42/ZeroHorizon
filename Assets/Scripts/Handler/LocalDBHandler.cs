using UnityEngine;

namespace Handler
{
    public static class LocalDBHandler
    {
        private const string KeyUID = "KEY_UID";
        private const string KeyNickname = "KEY_NICKNAME";

        public static void WriteUID(string uid)
        {
            PlayerPrefs.SetString(KeyUID, uid);
        }

        public static bool TryGetUID(out string result)
        {
            result = PlayerPrefs.GetString(KeyUID);
            return !string.IsNullOrEmpty(result);
        }

        public static void WriteNickname(string nickname)
        {
            PlayerPrefs.SetString(KeyNickname, nickname);
        }

        public static bool TryGetNickname(out string result)
        {
            result = PlayerPrefs.GetString(KeyNickname);
            return !string.IsNullOrEmpty(result);
        }
    }
}