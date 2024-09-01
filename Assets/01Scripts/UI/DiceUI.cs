using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BoardGame
{
    public class DiceUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField diceCountInputField,
            minDiceValueInputField,
            maxDiceValueInputField;

        [SerializeField]
        private Dropdown dicePresetDrowdown;

        [SerializeField]
        private Button buttonRollDice;

        public event Action<int> OnDiceCountUpdated,
            OnMaxDiceValueUpdated,
            OnMinDiceValueUpdated;
        public event Action OnDiceRolled;

        #region Singleton

        public static DiceUI Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        #endregion  Singleton

        private void Start()
        {
            CreateDicePresetDropdownOptions();
        }

        private void OnEnable()
        {
            buttonRollDice.onClick.AddListener(OnClickedButtonRollDice);
            diceCountInputField.onValueChanged.AddListener(OnDiceCountChanged);
            dicePresetDrowdown.onValueChanged.AddListener(OnDicePresetValueChanged);
            maxDiceValueInputField.onValueChanged.AddListener(OnMaxDiceValueChanged);
            minDiceValueInputField.onValueChanged.AddListener(OnMinDiceValueChanged);
        }

        private void OnDisable()
        {
            buttonRollDice.onClick.RemoveListener(OnClickedButtonRollDice);
            diceCountInputField.onValueChanged.RemoveListener(OnDiceCountChanged);
            dicePresetDrowdown.onValueChanged.RemoveListener(OnDicePresetValueChanged);
            maxDiceValueInputField.onValueChanged.RemoveListener(OnMaxDiceValueChanged);
            minDiceValueInputField.onValueChanged.RemoveListener(OnMinDiceValueChanged);
        }

        private void CreateDicePresetDropdownOptions()
        {
            List<Dropdown.OptionData> optionDatas = new();
            for (
                int i = DiceSettingsSO.Instance.MinDiceValue;
                i <= DiceSettingsSO.Instance.MaxDiceValue;
                i++
            )
            {
                optionDatas.Add(new Dropdown.OptionData(i.ToString()));
            }

            dicePresetDrowdown.AddOptions(optionDatas);
        }

        #region Events

        private void OnClickedButtonRollDice() 
        { 
            OnDiceRolled?.Invoke();
            buttonRollDice.interactable = false;
            PlayerManager.Instance.OnTurnEnded += OnTurnEnded;
        }

        private void OnDicePresetValueChanged(int presetCount) 
        { 
            //No need for converting int to string again and again.
            //Due to that, I don't call the Min-Max Value Changed functions.
            maxDiceValueInputField.SetTextWithoutNotify(dicePresetDrowdown.options[presetCount].text);
            minDiceValueInputField.SetTextWithoutNotify(DiceSettingsSO.Instance.MinDiceValue.ToString());

            OnMinDiceValueUpdated?.Invoke(DiceSettingsSO.Instance.MinDiceValue);
            OnMaxDiceValueUpdated?.Invoke(presetCount);
        }

        private void OnMaxDiceValueChanged(string maxString)
        {
            int maxValue = int.Parse(maxString);
            if (maxValue > DiceSettingsSO.Instance.MaxDiceValue)
                maxValue = DiceSettingsSO.Instance.MaxDiceValue;

            OnMaxDiceValueUpdated?.Invoke(maxValue);
        }

        private void OnMinDiceValueChanged(string minString)
        {
            int minValue = int.Parse(minString);
            if (minValue < DiceSettingsSO.Instance.MinDiceValue)
                minValue = DiceSettingsSO.Instance.MinDiceValue;

            OnMinDiceValueUpdated?.Invoke(minValue);
        }

        private void OnDiceCountChanged(string countString)
        {
            int diceCount = int.Parse(countString);
            diceCount = Mathf.Clamp(
                diceCount,
                DiceSettingsSO.Instance.MinDiceCount,
                DiceSettingsSO.Instance.MaxDiceCount
            );

            OnDiceCountUpdated?.Invoke(diceCount);
        }

        private void OnTurnEnded()
        {
            buttonRollDice.interactable = true;
            PlayerManager.Instance.OnTurnEnded -= OnTurnEnded;
        }

        #endregion Events
    }
}
