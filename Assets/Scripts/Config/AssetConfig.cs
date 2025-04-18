using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(menuName = "SO/Config/Asset/AssetConfig")]
    public class AssetConfig : SerializedScriptableObject
    {
        [SerializeField] private string[] _labels;
        
        public IReadOnlyCollection<string> labels => _labels;
    }
}