using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    #region Player
    [Header("Player Related")]
    public GameObject player;
    #endregion
    [Space]
    #region Platforms
    [Header("Platform Related")]
    public GameObject laneSpawnPlatformPrefab;
    public GameObject platformParentPrefab;
    public List<GameObject> platformParentObjects = new List<GameObject>();
    #endregion
}
