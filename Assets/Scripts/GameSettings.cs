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
    public float zMaxDifferenceBetweenObstacles = 35f;
    public float zMinDifferenceBetweenObstacles = 50f;
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
    public float zMaxDifferenceBetweenCollectibles = 2f;
    public float zMinDifferenceBetweenCollectibles = 5f;
    public float distanceMovingToPlayer = 20f;
    #endregion
    [Space]
    #region
    [Header("Jumping")]
    public float jumpDuration = 1f;  // Zýplamanýn toplam süresi (saniye cinsinden)
    public float jumpHeight = 3f;    // Maksimum zýplama yüksekliði
    #endregion
    [Space]
    #region
    [Header("Enviroment")]
    public float minOnTheRightSide = 4.0f;
    public float maxOnTheRightSide = 25.0f;
    public float minOnTheLeftSide = -4.0f;
    public float maxOnTheLeftSide = -30.0f;
    public float minHeight = -15.0f;
    public float maxHeight = 12.0f;
    public float minDifferenceBetweenBuilding = 20.0f;
    public float maxDifferenceBetweenBuilding = 50.0f;
    #endregion

    #region
    public const string OBSTACLE_TAG_STRING = "Obstacle";
    public const string COLLECTÝBLE_TAG_STRING = "Collectible";
    public const string BUILDING_TAG_STRING = "Building";
    #endregion

}
