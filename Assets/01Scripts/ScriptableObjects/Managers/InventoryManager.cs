using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    [CreateAssetMenu(
        fileName = "InventoryManager",
        menuName = "Inventory/InventoryManager",
        order = 1
    )]
    public class InventoryManager : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItemSO> inventoryItems;

        public List<InventoryItemSO> InventoryItems => inventoryItems;

        public event Action<string, int> OnItemQuantityChanged;

        #region Singleton

        public static InventoryManager Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void HandleInstance()
        {
            Instance = Resources.Load<InventoryManager>(Constants.Paths.PATH_INVENTORY_MANAGER);
            Instance.Initialize();
        }

        #endregion  Singleton

        private void Initialize()
        {
            inventoryItems.ForEach(x => x.Initialize());
        }

        public void UpdateQuantity(string name, int quantityChange)
        {
            InventoryItemSO item = inventoryItems.Find(x => x.InventoryItemModel.Name.Equals(name));

            if (item is not null)
            {
                item.UpdateQuantity(quantityChange);
                OnItemQuantityChanged?.Invoke(item.InventoryItemModel.Name, item.Quantity);
            }
        }
    }
}
