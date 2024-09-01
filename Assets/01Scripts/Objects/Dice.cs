using System;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

namespace BoardGame
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
    public class Dice : PoolObject
    {
        private bool _isDiceStopped = true;
        private List<DiceSide> _diceSides;
        private Rigidbody _rigidbody;

        public bool IsDiceStopped => _isDiceStopped;

        public event Action<Dice> OnDiceStopped;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _diceSides = new();
            if (_diceSides.Count == 0)
                GetComponentsInChildren(_diceSides);
        }

        private void OnCollisionStay(Collision other)
        {
            CheckIfStopped();
        }

        private List<int> GetRandomizedValues(int min, int max)
        {
            int num;
            List<int> numbers = new();

            if (max - min >= _diceSides.Count)
            {
                while (numbers.Count < _diceSides.Count)
                {
                    num = Utilities.GetRandomInt(min, max + 1);
                    if (!numbers.Contains(num))
                    {
                        numbers.Add(num);
                    }
                }
            }
            else
            {
                for (int i = 0; i < _diceSides.Count; i++)
                {
                    numbers.Add(Utilities.GetRandomInt(min, max + 1));
                }
            }

            return numbers;
        }

        private void AssignValuesToSides(List<int> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                _diceSides[i].UpdateNumber(values[i]);
            }
        }

        private void CheckIfStopped()
        {
            if (
                !_isDiceStopped
                && _rigidbody.velocity.magnitude < 0.01f
                && _rigidbody.angularVelocity.magnitude < 0.01f
            )
            {
                _isDiceStopped = true;
                _rigidbody.isKinematic = true;
                OnDiceStopped.Invoke(this);
            }
        }

        public int GetTopSideValue() 
        { 
            return _diceSides.Find(x => x.IsOnTop()).Number;
        }

        public void Throw(int min, int max)
        {
            AssignValuesToSides(GetRandomizedValues(min, max));
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(
                (Vector3.forward + Vector3.up) * DiceSettingsSO.Instance.DiceThrowForce,
                ForceMode.Impulse
            );
            _rigidbody.AddTorque(Utilities.GetRandomVector3());
            _isDiceStopped = false;
        }

        public override void ResetObject() { }
    }
}
