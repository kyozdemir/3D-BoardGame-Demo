using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class Cell : PoolObject
    {
        [Header("UI References")]
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Image nameBG;

        [SerializeField]
        private TMP_Text multiplierText,
            nameText;
        private Cell _nextCell;

        [Header("Component Reference")]
        [SerializeField]
        private MeshRenderer meshRenderer;

        [Header("Transform References")]
        [SerializeField]
        private Transform moveTransform;

        private bool _isEmpty;
        private MaterialPropertyBlock _mpb;
        private CellModel _cellModel;

        public Cell NextCell => _nextCell;
        public Transform MoveTransform => moveTransform;

        private void CustomizeCell()
        {
            if (string.IsNullOrEmpty(_cellModel.inventoryItemModel.Name))
            {
                DisplayUI(false);
            }
            iconImage.sprite = _cellModel.inventoryItemModel.Icon;
            multiplierText.SetText($"x{_cellModel.Quantity}");
            nameText.SetText($"{_cellModel.inventoryItemModel.Name}");
        }

        private void DisplayUI(bool canDisplay)
        {
            iconImage.gameObject.SetActive(canDisplay);
            multiplierText.gameObject.SetActive(canDisplay);
            nameBG.gameObject.SetActive(canDisplay);
        }

        public void InitializeCell(CellModel cellModel)
        {
            _cellModel = cellModel;
            CustomizeCell();
        }

        public void SetNextCell(Cell nextCell)
        {
            _nextCell = nextCell;
        }

        public void PlayerLanded(PlayerController player)
        {
            InventoryManager.Instance.UpdateQuantity(
                _cellModel.inventoryItemModel.Name,
                _cellModel.Quantity
            );
        }

        public void PlayerBounced()
        {
            transform.JumpToTargetAsync(transform.position, -.15f, .2f).Forget();
            meshRenderer.ChangeEmissionByTimeAsync(Color.white, .25f).Forget();
        }

        public override void ResetObject()
        {
            _nextCell = null;
            DisplayUI(true);
        }
    }
}
