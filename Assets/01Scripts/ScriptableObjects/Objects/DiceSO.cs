using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    [CreateAssetMenu(fileName = "DiceSO", menuName = "Dice/DiceSO", order = 0)]
    public class DiceSO : ScriptableObject
    {
        public string Name;
        public Dice DicePrefab;
        public Vector2 ValueRange;
    }
}
