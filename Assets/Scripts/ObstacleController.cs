using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private GameSettings _gameSettings;

    [SerializeField] 
    private PlayerSettings _playerSettings;

    [SerializeField]
    private CollectibleSettings _collectibleSettings;

    [SerializeField]
    private PlatformSettings _platformSettings;

    [SerializeField]
    private ObstacleSettings _obstacleSettings;

    public float lastObstaclePositionZ;
    private float _maxObstacleSpawnAhead = 150f;
    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();

      lastObstaclePositionZ = _gameSettings.player.transform.position.z +_playerSettings.distanceMovingToPlayer;
        SpawnObstacle();
    }

    private void Update()
    {
        if (lastObstaclePositionZ< _gameSettings.player.transform.position.z + _maxObstacleSpawnAhead)
        {
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        while (lastObstaclePositionZ < _gameSettings.player.transform.position.z + _maxObstacleSpawnAhead) 
        {
            List<int> allLaneIndices = new List<int>(); 

            for (int i = 0; i < _platformSettings.laneCount; i++) 
            {
                allLaneIndices.Add(i); 
            }

            allLaneIndices.Shuffle(); 

            int obstacleSpawnAmount = Random.Range(1, _platformSettings.laneCount ); 

            float zRandomOffset = Random.Range(_obstacleSettings.zMinDifferenceBetweenObstacles, _obstacleSettings.zMaxDifferenceBetweenObstacles);
            float newZPosition = lastObstaclePositionZ + zRandomOffset; 

            for (int i = 0; i < obstacleSpawnAmount; i++)
            {
                float xPosition = _platformSettings.firstLanePositionX + allLaneIndices[i] * _platformSettings.distanceBetweenLanes;
                GameObject spawnedObstacle = PoolManager.Instance.GetFromPool(ObstacleSettings.OBSTACLE_TAG_STRING);
                spawnedObstacle.transform.position = new Vector3(xPosition, 0, newZPosition);
            }

            lastObstaclePositionZ = newZPosition;
        }
    }
}
