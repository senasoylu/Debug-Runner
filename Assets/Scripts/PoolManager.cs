using System;
using System.Collections.Generic;
using UnityEngine;
using static PoolManager;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance; // Singleton
    //statik tutunca unic olur ve de�i�mez istedi�im zaman istedi�im yeden �a��rabilirim referanss�z kullanabilirim
    // statikte maaliyetlidir her yerde kullanmamal�y�z
    [System.Serializable]
    //GC yi az tetiklemek i�in kullanlan bir sistemdir
    public class Pool
    {
        public string tag;         // �rne�in "Bullet", "Obstacle", "Collectible", "X"
        public GameObject prefab;  // Havuzlanacak prefab
        public int size;           // Ba�lang�� boyutu
        public int extendSize;

        public Queue<GameObject> _gameObjectQueue;
    }

    public List<Pool> pools; // Inspector �zerinden havuz tan�mlamalar�n�z� yap�n
    public Dictionary<string, Pool> poolDictionary;
    private Transform _poolContainer;

    void Awake() //start de�il ��nk� starttan da �nce �a��r�l�r Execution Order da ilk s�radad�r
    {
        if (Instance == null)
        {
            Instance = this; // Singleton atamas�
           
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
        poolDictionary = new Dictionary<string, Pool>(); //ba��nda new varsa bu da maaliyeti etkiler

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
        if (!poolDictionary.ContainsKey(tag)) //Pool dict' de bu tag i ar�yor b�yle bir key var m� diyor
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        int count = poolDictionary[tag]._gameObjectQueue.Count; // "X" tagim de ka� objem var
        if (count <= 0) // tag de obje yoksa
        {
            ExtendPool(tag); //pool geni�lesin
        }
        GameObject objectToSpawn = poolDictionary[tag]._gameObjectQueue.Dequeue(); //b�t�n �retilen X tag�ndaki objeleri Dequeue ile listeden ��kar diyip �ne �ekiyosun
        objectToSpawn.SetActive(true); // g�r�nen obje oldum
        return objectToSpawn; //SpawnFromPool �a��r�nca GameObject d�nd�r�cem demek 
    }

    private void ExtendPool(string tag)
    {
        if (poolDictionary.TryGetValue(tag, out Pool pool)) //s�zl��e giriyorum ve i�inde bu tagden var m� diye soruyorum e�er varsa 
        {                                                   //��kt� Pool s�n�f�ndan POOl tagi ile oldu�unu do�ruluyorum
            for (int i = 0; i < pool.extendSize; i++) //geni�letti�im say� kadar
            {
                GameObject obj = Instantiate(pool.prefab); // �ret
                obj.SetActive(false); //kapat
                pool._gameObjectQueue.Enqueue(obj); //listeye al
                obj.transform.SetParent(_poolContainer);
            }
        }
    }

    public void ReturnToPool(string tag, GameObject obj) //"x"" tagli objem i�in
    {
        obj.SetActive(false); //false yap
        poolDictionary[tag]._gameObjectQueue.Enqueue(obj); //geri listeye al
    }
}
