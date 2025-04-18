﻿using System.Collections.Generic;
using Define;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(menuName = "SO/Config/UI/UIConfig")]
    public class UIConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<ClientEnum.EUIType, string> _assetKeys = new();
        [SerializeField] private Dictionary<ClientEnum.EUIType, int> _sortOrders = new();
        
        public IReadOnlyDictionary<ClientEnum.EUIType, string> AssetKeys => _assetKeys;
        public IReadOnlyDictionary<ClientEnum.EUIType, int> SortOrders => _sortOrders;
    }
}