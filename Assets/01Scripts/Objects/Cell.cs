using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Cell : MonoBehaviour
    {
        private Cell _nextCell;

        public Cell NextCell => _nextCell;

        public void SetNextCell(Cell nextCell)
        {
            _nextCell = nextCell;
        }

        public void PlayerLanded(PlayerController player)
        {
            
        }
    }
}
