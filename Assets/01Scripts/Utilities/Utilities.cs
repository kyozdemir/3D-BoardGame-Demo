using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoardGame
{
    public static class Utilities
    {
        //UniTask provides an efficient allocation free async/await integration for Unity.
        //If you eliminate me because I use UniTask, I respect that, but I will continue to improve myself with better practices.
        public static async UniTask JumpToTargetAsync(
            this Transform transform,
            Vector3 targetPosition,
            float jumpPower,
            float duration
        )
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
                transform.position = new Vector3(
                    currentHorizontalPosition.x,
                    startPosition.y + heightOffset,
                    currentHorizontalPosition.z
                );

                await UniTask.Yield(); // yield return null
            }
            transform.position = targetPosition;
        }

        public static async UniTask BounceScaleAsync(
            this Transform transform,
            Vector3 targetScale,
            float duration
        )
        {
            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0f;
            float frequency = Mathf.PI / duration;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                float scaleFactor = Mathf.Sin(frequency * elapsedTime);
                transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleFactor);

                await UniTask.Yield();
            }

            transform.localScale = initialScale;
        }

        private static int _randomIntSelector;

        public static int GetRandomInt(int min, int max)
        {
            _randomIntSelector = Random.Range(min, max);
            return _randomIntSelector;
        }

        public static Vector3 GetRandomVector3() 
        { 
            return new Vector3(
                GetRandomInt(0,360),
                GetRandomInt(0,360),
                GetRandomInt(0,360)
            );
        }
    }
}
