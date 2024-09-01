using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private List<PlayerController> players;
        private PlayerController _currentPlayer;
        private int turn;

        public event Action OnTurnEnded;

        #region Singleton

        public static PlayerManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        #endregion  Singleton

        private void OnEnable()
        {
            DiceManager.Instance.OnDiceThrowCompleted += PlayTurn;
        }

        private void OnDisable() 
        { 
            DiceManager.Instance.OnDiceThrowCompleted -= PlayTurn;
        }

        public void PlayTurn(int totalDice)
        {
            _currentPlayer = players[(turn + 1) % players.Count];
            _currentPlayer.OnMovementCompleted += OnMovementCompleted;
            _currentPlayer.Move(totalDice);
        }

        private void OnMovementCompleted()
        {
            _currentPlayer.OnMovementCompleted -= OnMovementCompleted;
            OnTurnEnded?.Invoke();
        }
    }
}
