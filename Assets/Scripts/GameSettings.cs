using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    #region Player
    [Header("Player Related")]
    public GameObject player;
    public float firstLanePositionX = -6.0f;
    public float distanceBetweenLanes = 5.0f;// �eritler aras� mesafe
    public int currentLaneIndex = 1; //ba�lang�� index
    public int platformCount;
    public int laneCount = 5; //�erit say�s� 
    public float platformLength = 100.0f;
    public float playerForwardSpeed = 10.0f;   // player ileri h�z�
    public float playerSidewaySpeed = 20.0f;   // player �erit de�i�tirme h�z�
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
    public float jumpDuration = 1f;  // Z�plaman�n toplam s�resi (saniye cinsinden)
    public float jumpHeight = 3f;    // Maksimum z�plama y�ksekli�i
    public bool isJumping = false;  // Oyuncu �u anda z�pl�yor mu?
    public float jumpTimer = 0f;    // Z�plama s�resi boyunca ge�en zaman
    public float groundY;           // Z�plama ba�lamadan �nce oyuncunun yer seviyesindeki y konumu
    #endregion
}
