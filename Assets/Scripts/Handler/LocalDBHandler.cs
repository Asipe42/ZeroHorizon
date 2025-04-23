using UnityEngine;

namespace Handler
{
    public static class LocalDBHandler
    {
        private const string KeyUID = "KEY_UID";

        public static void WriteUID(string uid)
        {
            PlayerPrefs.SetString(KeyUID, uid);
        }

        public static bool TryGetUID(out string result)
        {
            result = PlayerPrefs.GetString(KeyUID);
            return !string.IsNullOrEmpty(result);
        }
    }
}