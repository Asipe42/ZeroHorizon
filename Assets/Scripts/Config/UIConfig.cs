using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(menuName = "SO/Config/UI/UIConfig")]
    public class UIConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<UIType, string> _assetKeys = new();
        [SerializeField] private Dictionary<UIType, int> _sortOrders = new();
        
        public IReadOnlyDictionary<UIType, string> AssetKeys => _assetKeys;
        public IReadOnlyDictionary<UIType, int> SortOrders => _sortOrders;
    }
}