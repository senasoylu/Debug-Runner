using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    #region Player
    [Header("Player Related")]
    public GameObject player;
    public float firstLanePositionX = -6.0f;
    public float distanceBetweenLanes = 5.0f;// þeritler arasý mesafe
    public int currentLaneIndex = 1; //baþlangýç index
    public int platformCount;
    public int laneCount = 5; //þerit sayýsý 
    public float platformLength = 100.0f;
    public float playerForwardSpeed = 10.0f;   // player ileri hýzý
    public float playerSidewaySpeed = 20.0f;   // player þerit deðiþtirme hýzý
    #endregion"
    [Space]
    #region Obstacles
    [Header("Obstacle Related")]
    public int obstacleCount;
    public GameObject obstaclePrefab;
    public GameObject obstaclePrefab2;
    public List<GameObject> obstacleObjects2=new List<GameObject>();
    public List<GameObject> ObstacleObjects = new List<GameObject>();
    public float LastObstaclePositionZ;
    #endregion
    [Space]
    #region Platforms
    [Header("Platform Related")]
    public GameObject laneSpawnPlatformPrefab;
    public GameObject platformParentPrefab;
    public List<GameObject> platformParentObjects = new List<GameObject>();
    #endregion
    [Space]
    #region Collectibles
    [Header("Collectibles Related")]
    public GameObject collectiblePrefab;
    public int collectibleCount = 20;
    public List<GameObject> CollectibleObjects = new List<GameObject>();
    public float lastCollectiblePositionZ;
    public float zMaxDifferenceBetweenCollectibles = 2f;
    public float zMinDifferenceBetweenCollectibles = 5f;
    public float distanceMovingToPlayer = 20f;
    #endregion
    [Space]
    #region
    [Header("Jumping")]
    public float jumpDuration = 1f;  // Zýplamanýn toplam süresi (saniye cinsinden)
    public float jumpHeight = 3f;    // Maksimum zýplama yüksekliði
    public bool isJumping = false;  // Oyuncu þu anda zýplýyor mu?
    public float jumpTimer = 0f;    // Zýplama süresi boyunca geçen zaman
    public float groundY;           // Zýplama baþlamadan önce oyuncunun yer seviyesindeki y konumu
    #endregion
}
