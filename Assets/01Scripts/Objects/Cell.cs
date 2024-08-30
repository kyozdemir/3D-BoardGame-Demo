using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class Cell : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text multiplierText, nameText;
        private Cell _nextCell;

        [Header("Component Reference")]
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Transform References")]
        [SerializeField] private Transform moveTransform;

        private MaterialPropertyBlock _mpb;
        private CellModel _cellModel;

        public Cell NextCell => _nextCell;
        public Transform MoveTransform => moveTransform;

        private void CustomizeCell()
        {
            iconImage.sprite = _cellModel.inventoryItemModel.Icon;
            multiplierText.SetText($"x{_cellModel.Quantity}");
            nameText.SetText($"{_cellModel.inventoryItemModel.Name}");
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

        }
    }
}
