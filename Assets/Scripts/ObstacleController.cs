using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private PlatformSettings _platformSettings;

    [SerializeField]
    private ObstacleSettings _obstacleSettings;

    public float lastObstaclePositionZ;
    
    private void Start()
    {
      lastObstaclePositionZ = _playerController.transform.position.z +_obstacleSettings.distanceMovingToPlayer;
       SpawnObstacle();
    }

    private void Update()
    {
        if (lastObstaclePositionZ< _playerController.transform.position.z + _obstacleSettings.maxSpawnDistanceAhead)
        {
          SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        while (lastObstaclePositionZ < _playerController.transform.position.z + _obstacleSettings.maxSpawnDistanceAhead) 
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

                spawnedObstacle.GetComponent<Obstacle>().SetPlayerTransform(_playerController.transform);
                spawnedObstacle.transform.position = new Vector3(xPosition, 0, newZPosition);
            }

            lastObstaclePositionZ = newZPosition;
        }
    }
}
