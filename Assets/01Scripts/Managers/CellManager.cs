using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class CellManager : MonoBehaviour
    {
        [SerializeField] private List<Cell> cells;
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private float spacing;
        [SerializeField] private int count;

        #region Singleton

        public static CellManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        #endregion


        void Start()
        {
            PoolManager.Instance.CreatePool<Cell>(Constants.PoolKeys.CELL, cellPrefab, 10);
            CreateCells();
            SetCellConnections();
        }

        private void CreateCells()
        {
            for (int i = 0; i < count; i++)
            {
                Cell newCell = PoolManager.Instance.GetObject<Cell>(Constants.PoolKeys.CELL, Vector3.zero + (Vector3.forward * spacing * i));
                InventoryItemModel cellItemModel = InventoryManager.Instance.InventoryItems[UnityEngine.Random.Range(0, InventoryManager.Instance.InventoryItems.Count)].InventoryItemModel;
                newCell.InitializeCell(new CellModel
                {
                     inventoryItemModel = cellItemModel,
                     Quantity = UnityEngine.Random.Range(0, 10)
                });
                cells.Add(newCell);
            }
        }

        void SetCellConnections()
        {
            for (int i = 0; i < cells.Count - 1; i++)
            {
                cells[i].SetNextCell(cells[i + 1]);
            }

            cells[cells.Count - 1].SetNextCell(cells[0]);
        }

        public Cell GetCellByIndex(int index)
        {
            if (index < 0) return null;
            if (index > cells.Count - 1) return cells[0];
            return cells[index];
        }
    }
}
