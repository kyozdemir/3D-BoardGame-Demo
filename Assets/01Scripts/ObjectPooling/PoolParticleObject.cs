using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PoolParticleObject : PoolObject
    {
        private Coroutine _ctrParticle;
        private ParticleSystem _particleSystem;
        public event Action<PoolParticleObject> OnParticleFinished;

        void Start()
        {
            if (_particleSystem is null)
                _particleSystem = GetComponent<ParticleSystem>();

            if(_ctrParticle is null)
            _ctrParticle = StartCoroutine(WaitUnitStop());
        }

        private IEnumerator WaitUnitStop()
        {
            yield return new WaitUntil(() => !_particleSystem.isPlaying);
            _ctrParticle = null;
            OnParticleFinished?.Invoke(this);
        }

        public override void ResetObject(){}
    }
}
