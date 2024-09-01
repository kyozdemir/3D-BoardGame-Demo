using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BoardGame
{
    public class DiceSide : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text numberText;
        private int _number;

        public int Number => _number;

        public bool IsOnTop()
        {
            return Vector3.Dot(-transform.forward, Vector3.up) >= .9f;
        }

        public void UpdateNumber(int number)
        {
            _number = number;
            UpdateNumberText();
        }

        private void UpdateNumberText()
        {
            numberText.SetText(_number.ToString());
        }
    }
}
