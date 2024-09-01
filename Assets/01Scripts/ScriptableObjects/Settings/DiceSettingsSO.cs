using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    [CreateAssetMenu(fileName = "DiceSettings", menuName = "Dice/DiceSettings", order = 1)]
    public class DiceSettingsSO : ScriptableObject
    {
        [SerializeField] private float diceThrowForce;
        [SerializeField] private int initialDiceCount, maxDiceCount, minDiceCount;
        [SerializeField] private int initialDiceValue, minDiceValue, maxDiceValue;

        public float DiceThrowForce => diceThrowForce;
        public int InitialDiceCount => initialDiceCount;
        public int InitialDiceValue => initialDiceValue;
        public int MaxDiceCount => maxDiceCount;
        public int MinDiceCount => minDiceCount;
        public int MaxDiceValue => maxDiceValue;
        public int MinDiceValue => minDiceValue;

        #region Singleton

        public static DiceSettingsSO Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void HandleInstance()
        {
            Instance = Resources.Load<DiceSettingsSO>(Constants.Paths.PATH_DICE_SETTINGS);
        }

        #endregion  Singleton
    }
}
