using System;
using System.Collections.Generic;
using UnityEngine;
using static PoolManager;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance; // Singleton
    //statik tutunca unic olur ve deðiþmez istediðim zaman istediðim yeden çaðýrabilirim referanssýz kullanabilirim
    // statikte maaliyetlidir her yerde kullanmamalýyýz
    [System.Serializable]
    //GC yi az tetiklemek için kullanlan bir sistemdir
    public class Pool
    {
        public string tag;         // Örneðin "Bullet", "Obstacle", "Collectible", "X"
        public GameObject prefab;  // Havuzlanacak prefab
        public int size;           // Baþlangýç boyutu
        public int extendSize;

        public Queue<GameObject> _gameObjectQueue;
    }

    public List<Pool> pools; // Inspector üzerinden havuz tanýmlamalarýnýzý yapýn
    public Dictionary<string, Pool> poolDictionary;
    private Transform _poolContainer;

    void Awake() //start deðil çünkü starttan da önce çaðýrýlýr Execution Order da ilk sýradadýr
    {
        if (Instance == null)
        {
            Instance = this; // Singleton atamasý
           
        }
        else
        {
            Destroy(gameObject);
        }
        _poolContainer = new GameObject("PoolParent").transform;
        Initialize();

    }

    void Start()
    {
        
    }

    private void Initialize() 
    {
        poolDictionary = new Dictionary<string, Pool>(); //baþýnda new varsa bu da maaliyeti etkiler

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
        if (!poolDictionary.ContainsKey(tag)) //Pool dict' de bu tag i arýyor böyle bir key var mý diyor
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        int count = poolDictionary[tag]._gameObjectQueue.Count; // "X" tagim de kaç objem var
        if (count <= 0) // tag de obje yoksa
        {
            ExtendPool(tag); //pool geniþlesin
        }
        GameObject objectToSpawn = poolDictionary[tag]._gameObjectQueue.Dequeue(); //bütün üretilen X tagýndaki objeleri Dequeue ile listeden çýkar diyip öne çekiyosun
        objectToSpawn.SetActive(true); // görünen obje oldum
        return objectToSpawn; //SpawnFromPool çaðýrýnca GameObject döndürücem demek 
    }

    private void ExtendPool(string tag)
    {
        if (poolDictionary.TryGetValue(tag, out Pool pool)) //sözlüðe giriyorum ve içinde bu tagden var mý diye soruyorum eðer varsa 
        {                                                   //çýktý Pool sýnýfýndan POOl tagi ile olduðunu doðruluyorum
            for (int i = 0; i < pool.extendSize; i++) //geniþlettiðim sayý kadar
            {
                GameObject obj = Instantiate(pool.prefab); // üret
                obj.SetActive(false); //kapat
                pool._gameObjectQueue.Enqueue(obj); //listeye al
                obj.transform.SetParent(_poolContainer);
            }
        }
    }

    public void ReturnToPool(string tag, GameObject obj) //"x"" tagli objem için
    {
        obj.SetActive(false); //false yap
        poolDictionary[tag]._gameObjectQueue.Enqueue(obj); //geri listeye al
    }
}
