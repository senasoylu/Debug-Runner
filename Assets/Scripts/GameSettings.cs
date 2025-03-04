using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // Start is called before the first frame update

    public float firstLanePositionX = -6.0f;
    public float distanceBetweenLanes = 5.0f;// þeritler arasý mesafe
    public int currentLaneIndex = 1; //baþlangýç index
    public int platformCount;
    public int laneCount = 5; //þerit sayýsý 
    public float platformLength = 100.0f;
    public float playerForwardSpeed = 10.0f;   // player ileri hýzý
    public float playerSidewaySpeed = 20.0f;   // player þerit deðiþtirme hýzý
    //
   
    public int obstacleCount;
    public GameObject obstaclePrefab;
   // public int zDifferenceBetweenObstacle;
    public List<GameObject> ObstacleObjects = new List<GameObject>();
    public float LastObstaclePositionZ;

    //

    public GameObject player;
    public GameObject laneSpawnPlatformPrefab;
    public GameObject platformParentPrefab;
    public List<GameObject> platformParentObjects = new List<GameObject>();


    public GameObject collectiblePrefab;
    public int collectibleCount = 20;
    //public int zDifferenceBetweenCollectibles;
    public List<GameObject> CollectibleObjects=new List<GameObject>();
    public float lastCollectiblePositionZ;


    




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
