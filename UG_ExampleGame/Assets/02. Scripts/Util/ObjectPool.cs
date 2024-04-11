using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UK.Util
{
    [Serializable]
    public struct PoolItem
    {
        public GameObject   prefab;
        public int          size;
    }

    public class ObjectPool : MonoSingleton<ObjectPool>
    {
        [SerializeField]
        private List<PoolItem> poolItems;
        private readonly int initAddSize = 1;

        private Dictionary<GameObject, GameObject>          spawnContainer = new Dictionary<GameObject, GameObject>();
        private Dictionary<GameObject, Stack<GameObject>>   poolContainers = new Dictionary<GameObject, Stack<GameObject>>();

        private void Awake()
        {
            for(int i = 0; i < poolItems.Count; ++i)
                CreatePool(poolItems[i].prefab, poolItems[i].size);
        }

        public void CreatePool(GameObject prefab, int size)
        {
            if(poolContainers.ContainsKey(prefab) == false)
                poolContainers.Add(prefab, new Stack<GameObject>()); 

            for(int i = 0; i < size; ++i)
            {
                var newObject = Instantiate(prefab, transform);
                newObject.SetActive(false);
                poolContainers[prefab].Push(newObject);
            }
        }

        public void RemovePool(GameObject prefab)
        {
            if (poolContainers.ContainsKey(prefab) == false)
                return;

            while (poolContainers[prefab].Count > 0)
            {
                var top = poolContainers[prefab].Pop();
                Destroy(top);
            }    
        }

        public void RemoveAllPool()
        {
            List<GameObject> tempList = new List<GameObject>();
            foreach (var item in poolContainers.Keys)
            {
                tempList.Add(item);
            }
            for (int i = 0; i < tempList.Count; ++i)
                RemovePool(tempList[i]);
        }

        public GameObject SpawnObject(GameObject prefab, Transform parent = null)
        {
            if (poolContainers.ContainsKey(prefab) == false || poolContainers[prefab].Count == 0)
            {
                CreatePool(prefab, initAddSize);
            }

            var spawnObject = poolContainers[prefab].Pop();
            spawnObject.transform.SetParent(parent);
            spawnObject.transform.localPosition = Vector3.zero;
            spawnObject.SetActive(true);
            spawnContainer.Add(spawnObject, prefab);
            return spawnObject;
        }

        public void DespawnObject(GameObject spawnObject)
        {
            if (spawnContainer.ContainsKey(spawnObject) == false)
                return;

            var prefab = spawnContainer[spawnObject];
            spawnObject.transform.SetParent(transform);
            spawnObject.SetActive(false);
            spawnContainer.Remove(spawnObject);
            poolContainers[prefab].Push(spawnObject);
        }


        public T SpawnObject<T>(T prefab, Transform parent = null) where T : Component
        {
            return prefab.gameObject.SpawnObject(parent).GetComponent<T>();
        }

        public void DespawnObject<T>(T spawnObject) where T : Component
        {
            spawnObject.gameObject.DespawnObject();
        }
    }

    public static class ObjectPoolExtension
    {
        public static GameObject SpawnObject(this GameObject prefab, Transform parent = null)
        {
            return ObjectPool.Instance.SpawnObject(prefab.gameObject, parent);
        }

        public static void DespawnObject(this GameObject spawnObject)
        {
            ObjectPool.Instance.DespawnObject(spawnObject);
        }

        public static async UniTask DespawnObject(this GameObject spawnObject, float delayTime)
        {
            await UniTask.WaitForSeconds(delayTime);
            ObjectPool.Instance.DespawnObject(spawnObject);
        }

        public static T SpawnObject<T>(this T prefab, Transform parent = null) where T : Component
        {
            return ObjectPool.Instance.SpawnObject<T>(prefab);
        }

        public static void DespawnObject<T>(this T spawnObject) where T : Component
        {
            ObjectPool.Instance.DespawnObject(spawnObject);
        }
    }
}
