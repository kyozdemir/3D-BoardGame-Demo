using System;
using UnityEngine;

namespace BoardGame
{
    public class PlayerController : MonoBehaviour
    {
        private Cell _currentCell;
        private int _remainingStep;

        public event Action OnMovementCompleted;

        private void Start()
        {
            _currentCell = CellManager.Instance.GetCellByIndex(0);
            transform.position = _currentCell.MoveTransform.position;
        }

        private async void MoveCells()
        {
            int count = _remainingStep;
            for (int i = 0; i < count; i++)
            {
                await transform.JumpToTargetAsync(
                    _currentCell.NextCell.MoveTransform.position,
                    1f,
                    .5f
                );
                _remainingStep--;
                _currentCell = _currentCell.NextCell;
            }

            if (_remainingStep == 0)
            {
                _currentCell.PlayerLanded(this);
                OnMovementCompleted?.Invoke();
            }
        }

        public void Move(int remainingStep) 
        {
            _remainingStep = remainingStep;
            MoveCells();
        }
    }
}
