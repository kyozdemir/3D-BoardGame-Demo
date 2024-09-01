using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class FollowTargetWithOffset : MonoBehaviour
    {
        [SerializeField] private bool useYOffset;
        [SerializeField] private float speed;
        [SerializeField] private Vector3 followOffset;
        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            if(target is null) return;
            transform.position = Vector3.Lerp(
                transform.position,
                (useYOffset ? target.position : new Vector3(target.position.x, transform.position.y, target.position.z)) + followOffset,
                Time.deltaTime * speed
            );
        }
    }
}
