using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    private PlatformSettings _platformSettings;

    [SerializeField]
    private ObstacleSettings _obstacleSettings;

    [SerializeField]
   private PlayerNavigationData _playerNavigationData;

    private float _lastObstaclePositionZ;
    
    private void Start()
    {
      _lastObstaclePositionZ = _playerNavigationData.GetPlayerPosition().z +_obstacleSettings.distanceMovingToPlayer;
       SpawnObstacle();
    }

    private void Update()
    {
        if (_lastObstaclePositionZ< _playerNavigationData.GetPlayerPosition().z + _obstacleSettings.maxSpawnDistanceAhead)
        {
          SpawnObstacle();
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(ObstacleSettings.OBSTACLE_TAG_STRING))
        {
            if (obj.transform.position.z < _playerNavigationData.GetPlayerPosition().z - _obstacleSettings.distanceMovingToPlayer)
            {
                PoolManager.Instance.ReturnToPool(ObstacleSettings.OBSTACLE_TAG_STRING, obj);
            }
        }
    }

    private void SpawnObstacle()
    {
        while (_lastObstaclePositionZ < _playerNavigationData.GetPlayerPosition().z + _obstacleSettings.maxSpawnDistanceAhead) 
        {
            List<int> allLaneIndices = new List<int>(); 

            for (int i = 0; i < _platformSettings.laneCount; i++) 
            {
                allLaneIndices.Add(i); 
            }

            allLaneIndices.Shuffle(); 

            int obstacleSpawnAmount = Random.Range(1, _platformSettings.laneCount ); 

            float zRandomOffset = Random.Range(_obstacleSettings.zMinDifferenceBetweenObstacles, _obstacleSettings.zMaxDifferenceBetweenObstacles);
            float newZPosition = _lastObstaclePositionZ + zRandomOffset; 

            for (int i = 0; i < obstacleSpawnAmount; i++)
            {
                float xPosition = _platformSettings.firstLanePositionX + allLaneIndices[i] * _platformSettings.distanceBetweenLanes;
                GameObject spawnedObstacle = PoolManager.Instance.GetFromPool(ObstacleSettings.OBSTACLE_TAG_STRING);

                spawnedObstacle.transform.position = new Vector3(xPosition, 0, newZPosition);
            }

            _lastObstaclePositionZ = newZPosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Collector"))
        {
            CubeManager.Instance?.DropLastCube();
        }
    }
}
