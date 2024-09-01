using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace BoardGame
{
    public class DiceManager : MonoBehaviour
    {
        [Header("Dice References")]
        [SerializeField]
        private List<DiceSO> dices;

        [SerializeField]
        private Transform diceThrowStartingTransform;

        private bool _isThrowReady;
        private DiceSO _selectedDiceSO;
        private int _diceCount,
            _totalDice,
            _stoppedDiceCount;
        private List<Dice> _createdDices;
        private Vector2 _diceRange;

        public event Action<int> OnDiceThrowCompleted;

        #region Singleton

        public static DiceManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            dices.ForEach(x => PoolManager.Instance.CreatePool<Dice>(x.Name, x.DicePrefab, 0));
        }

        #endregion  Singleton

        private void Start()
        {
            _selectedDiceSO = dices[0];
            _createdDices = new();
            _diceCount = DiceSettingsSO.Instance.InitialDiceCount;
            _diceRange = new Vector2(
                DiceSettingsSO.Instance.MinDiceValue,
                DiceSettingsSO.Instance.InitialDiceValue
            );
        }

        private void OnEnable()
        {
            DiceUI.Instance.OnDiceCountUpdated += OnDiceCountUpdated;
            DiceUI.Instance.OnMaxDiceValueUpdated += OnMaxDiceValueUpdated;
            DiceUI.Instance.OnMinDiceValueUpdated += OnMinDiceValueUpdated;
            DiceUI.Instance.OnDiceRolled += OnDiceRolled;
        }

        private void OnDisable()
        {
            DiceUI.Instance.OnDiceCountUpdated -= OnDiceCountUpdated;
            DiceUI.Instance.OnMaxDiceValueUpdated -= OnMaxDiceValueUpdated;
            DiceUI.Instance.OnMinDiceValueUpdated -= OnMinDiceValueUpdated;
            DiceUI.Instance.OnDiceRolled -= OnDiceRolled;
        }

        #region Add/Remove

        private void CreateNewDiceByCount(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Dice createdDice = PoolManager.Instance.GetObject<Dice>(
                    _selectedDiceSO.name,
                    diceThrowStartingTransform.position
                    + (Vector3.forward * (i))
                );
                _createdDices.Add(createdDice);
            }
        }

        private void RemoveDiceByCount(int count = 1)
        {
            if (count > _createdDices.Count)
            {
                Debug.LogWarning(
                    $"Removing dice is forbidden. Remove count {count} is bigger than dice count {_createdDices.Count}"
                );
                return;
            }
            for (int i = 0; i < count; i++)
            {
                PoolManager.Instance.ReturnObject<Dice>(_selectedDiceSO.Name, _createdDices[0]);
                _createdDices.RemoveAt(0);
            }
        }

        #endregion Add/Remove

        #region Throwing Dice

        public void ThrowDice()
        {
            if (_diceCount < _createdDices.Count)
            {
                RemoveDiceByCount(_createdDices.Count - _diceCount);
            }
            else if (_diceCount > _createdDices.Count)
            {
                CreateNewDiceByCount(_diceCount - _createdDices.Count);
            }

            for (int i = 0; i < _createdDices.Count; i++)
            {
                _createdDices[i].OnDiceStopped += OnDiceStopped;
                _createdDices[i].transform.position =
                    diceThrowStartingTransform.position
                    + (Vector3.forward * (i + 2));
                _createdDices[i].Throw((int)_diceRange.x, (int)_diceRange.y);
            }

            _createdDices.ForEach(x => { });
        }

        #endregion Throwing Dice

        #region Events

        private void OnDiceStopped(Dice dice)
        {
            Debug.Log("Dice stopped");
            ++_stoppedDiceCount;
            dice.OnDiceStopped -= OnDiceStopped;
            _totalDice += dice.GetTopSideValue();

            Debug.Log(_stoppedDiceCount);
            Debug.Log(_createdDices.Count);
            if (_stoppedDiceCount == _createdDices.Count)
            {
                OnDiceThrowCompleted?.Invoke(_totalDice);
            }
        }

        private void OnDiceRolled()
        {
            _totalDice = _stoppedDiceCount = 0;
            ThrowDice();
        }

        private void OnDiceCountUpdated(int diceCount)
        {
            _diceCount = diceCount;
        }

        private void OnMaxDiceValueUpdated(int maxValue)
        {
            _diceRange.y = maxValue;
        }

        private void OnMinDiceValueUpdated(int minValue)
        {
            _diceRange.x = minValue;
        }

        #endregion
    }
}
