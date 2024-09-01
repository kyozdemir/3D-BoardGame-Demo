using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class InventoryDisplayer : MonoBehaviour
    {
        [Header("Prefab References")]
        [SerializeField]
        private ItemDisplayer itemDisplayerPrefab;

        private List<ItemDisplayer> _itemDisplayers;

        private void Start()
        {
            _itemDisplayers = new List<ItemDisplayer>();

            PoolManager.Instance.CreatePool<ItemDisplayer>(
                Constants.PoolKeys.ITEM_DISPLAYER,
                itemDisplayerPrefab,
                3
            );

            CreateDisplayers();

            InventoryManager.Instance.OnItemQuantityChanged += OnItemQuantityChanged;
        }

        private void CreateDisplayers()
        {
            List<InventoryItemSO> inventoryItems = InventoryManager.Instance.InventoryItems;
            foreach (InventoryItemSO item in inventoryItems)
            {
                if(string.IsNullOrEmpty(item.InventoryItemModel.Name)) continue;
                
                ItemDisplayer itemDisplayer = PoolManager.Instance.GetObject<ItemDisplayer>(
                    Constants.PoolKeys.ITEM_DISPLAYER,
                    default,
                    default,
                    transform
                );
                itemDisplayer.Initialize(item.InventoryItemModel, item.Quantity);
                _itemDisplayers.Add(itemDisplayer);
            }
        }

        private void OnItemQuantityChanged(string name, int quantity)
        {
            ItemDisplayer itemDisplayer = _itemDisplayers.Find(x =>
                x.InventoryItemModel.Name.Equals(name)
            );
            itemDisplayer.UpdateQuantityText(quantity, true);
        }
    }
}
