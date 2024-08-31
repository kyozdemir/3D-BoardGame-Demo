using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


namespace BoardGame
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        private Dictionary<string, object> _pools = new Dictionary<string, object>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool ContainsKey(string key)
        {
            return _pools.ContainsKey(key);
        }

        public void CreatePool<T>(string key, T prefab, int initialSize) where T : PoolObject
        {
            if (!_pools.ContainsKey(key))
            {
                ObjectPool<T> pool = new ObjectPool<T>(
                    createFunc: () =>
                    {
                        T obj = Instantiate(prefab);
                        obj.gameObject.SetActive(false);
                        return obj;
                    },
                    obj => obj.gameObject.SetActive(true),
                    obj => {
                        obj.gameObject.SetActive(false);
                        obj.ResetObject();
                    },
                    obj => Destroy(obj.gameObject)
                );

                _pools[key] = pool;
            }
        }

        public T GetObject<T>(string key, Vector3 position = default, Vector3 rotation = default, Transform parent = null) where T : PoolObject
        {
            if (_pools.ContainsKey(key))
            {
                ObjectPool<T> pool = _pools[key] as ObjectPool<T>;

                if (pool == null)
                {
                    Debug.LogError($"Pool for key {key} is not of type ObjectPool<{typeof(T).Name}>.");
                    return null;
                }

                T obj = pool.Get();
                obj.transform.SetParent(parent);
                obj.transform.SetLocalPositionAndRotation(position, Quaternion.Euler(rotation));
                return obj;
            }

            Debug.LogError($"Pool for key {key} is not FOUND of type ObjectPool<{typeof(T).Name}>.");
            return null;
        }

        public void ReturnObject<T>(string key, T obj) where T : PoolObject
        {
            if (_pools.ContainsKey(key))
            {
                ObjectPool<T> pool = _pools[key] as ObjectPool<T>;
                pool.Release(obj);
            }
        }
    }
}

