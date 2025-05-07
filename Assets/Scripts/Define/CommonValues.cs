using UnityEngine;

namespace Define
{
    public static class Layer
    {
        public static int Ground => 1 << LayerMask.NameToLayer("Ground");
    }
}