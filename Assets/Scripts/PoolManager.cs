using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance; 

    [System.Serializable]

    public class Pool
    {
        public GameObject prefab;

        public string tag;

        public int size;          
        public int extendSize;

        public Queue<GameObject> _gameObjectQueue;
    }

    public List<Pool> pools; 
    public Dictionary<string, Pool> poolDictionary;

    private Transform _poolContainer;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this; 
        }

        else
        {
            Destroy(gameObject);
        }

        _poolContainer = new GameObject("PoolParent").transform;

        Initialize();
    }

    private void Initialize()
    {
        poolDictionary = new Dictionary<string, Pool>(); 

        foreach (Pool pool in pools)
        {
            pool._gameObjectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);

                obj.SetActive(false);
                pool._gameObjectQueue.Enqueue(obj);
                obj.transform.SetParent(_poolContainer);
            }

            poolDictionary.Add(pool.tag, pool);
        }
    }

    public GameObject GetFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        int count = poolDictionary[tag]._gameObjectQueue.Count; 
        if (count <= 0) 
        {
            ExtendPool(tag);
        }
        GameObject objectToSpawn = poolDictionary[tag]._gameObjectQueue.Dequeue();
        objectToSpawn.SetActive(true); 
        return objectToSpawn; 
    }

    private void ExtendPool(string tag)
    {
        if (poolDictionary.TryGetValue(tag, out Pool pool)) 
        {                                                  
            for (int i = 0; i < pool.extendSize; i++) 
            {
                GameObject obj = Instantiate(pool.prefab); 
                obj.SetActive(false); 
                pool._gameObjectQueue.Enqueue(obj); 
                obj.transform.SetParent(_poolContainer);
               _poolContainer.position = Vector3.zero;
            }
        }
    }

    public void ReturnToPool(string tag, GameObject obj) 
    {
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
       
        poolDictionary[tag]._gameObjectQueue.Enqueue(obj);
    }
}
