using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(menuName = "SO/Config/UI/UIConfig")]
    public class UIConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<EUIType, string> _assetKeys = new();
        [SerializeField] private Dictionary<EUIType, int> _sortOrders = new();
        
        public IReadOnlyDictionary<EUIType, string> AssetKeys => _assetKeys;
        public IReadOnlyDictionary<EUIType, int> SortOrders => _sortOrders;
    }
}