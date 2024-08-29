using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BoardGame
{
    public class PlayerController : MonoBehaviour
    {
        private Cell _currentCell;
        private int _remainingStep;

        private void Start()
        {
            _currentCell = CellManager.Instance.GetCellByIndex(0);
            transform.position = _currentCell.transform.position;
        }

        private async void MoveCells()
        {
            int count = _remainingStep;
            for (int i = 0; i < count; i++)
            {
                await JumpToTargetAsync(_currentCell.NextCell.transform.position, 1f, .5f);
                _remainingStep--;
                _currentCell = _currentCell.NextCell;
            }

            if (_remainingStep == 0)
                _currentCell.PlayerLanded(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                _remainingStep = 5;
                MoveCells();
            }
        }

        #region Movement

        private async UniTask JumpToTargetAsync(Vector3 targetPosition, float jumpPower, float duration)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Lineer vertical movement
                Vector3 currentHorizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

                // Parabolic horizontal movement
                float heightOffset = jumpPower * Mathf.Sin(Mathf.PI * t);

                // Set new position
                transform.position = new Vector3(currentHorizontalPosition.x, startPosition.y + heightOffset, currentHorizontalPosition.z);

                await UniTask.Yield(); // yield return null
            }
            transform.position = targetPosition;
        }

        #endregion Movement

        #region Events



        #endregion Events
    }
}
