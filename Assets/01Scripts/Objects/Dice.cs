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
        private Vector3 direction = Vector3.zero;

        public bool IsDiceStopped => _isDiceStopped;

        public event Action<Dice> OnDiceStopped;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _diceSides = new();
            if (_diceSides.Count == 0)
                GetComponentsInChildren(_diceSides);
        }

        private void FixedUpdate()
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
            if (_rigidbody.IsSleeping())
                _rigidbody.WakeUp();
            if (
                !_isDiceStopped
                && _rigidbody.velocity.magnitude < 0.01f
                && _rigidbody.angularVelocity.magnitude < 0.01f
            )
            {
                Debug.Log("CHECK COMPLETE");
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

            float horizontalDistance =
                PlayerManager.Instance.CurrentPlayer.transform.position.x - transform.position.x;

            float gravity = Mathf.Abs(Physics.gravity.y);
            float time = Mathf.Sqrt(2 * 1 / gravity);

            float horizontalSpeed = (horizontalDistance / time) / 3;

            Vector3 velocity = new Vector3(horizontalDistance, 0, 0);

            _rigidbody.velocity = velocity;
            _rigidbody.AddTorque(Utilities.GetRandomVector3());
            _isDiceStopped = false;
        }

        public override void ResetObject() { }
    }
}
