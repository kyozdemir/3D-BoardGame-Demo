using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class ItemDisplayer : PoolObject
    {
        [Header("UI References")]
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private TMP_Text quantityText;

        private InventoryItemModel _inventoryItemModel;

        public InventoryItemModel InventoryItemModel => _inventoryItemModel;

        public void Initialize(InventoryItemModel inventoryItemModel, int quantity)
        {
            _inventoryItemModel = inventoryItemModel;
            UpdateQuantityText(quantity);
        }

        public override void ResetObject() { }

        public void UpdateQuantityText(int quantity, bool canBounce = false)
        {
            quantityText.SetText(quantity.ToString());

            if (canBounce)
                quantityText.transform.BounceScaleAsync(Vector3.one * 1.5f, .5f).Forget();
        }
    }
}
