using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    [CreateAssetMenu(fileName = "InventoryItemSO", menuName = "Inventory/InventoryItem", order = 0)]
    public class InventoryItemSO : ScriptableObject
    {
        public InventoryItemModel InventoryItemModel;
        private int _quantity;

        public int Quantity => _quantity;

        public void Initialize()
        {
            LoadQuantity();
        }

        public void UpdateQuantity(int value)
        {
            _quantity += value;
            SaveQuantity();
        }

        public void SaveQuantity()
        {
            PlayerPrefs.SetInt(InventoryItemModel.Name, _quantity);
            PlayerPrefs.Save();
        }

        private void LoadQuantity()
        {
            _quantity = PlayerPrefs.GetInt(InventoryItemModel.Name, 0);
        }
    }
}
