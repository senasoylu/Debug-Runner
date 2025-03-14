using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleController : MonoBehaviour
{
    private GameSettings _gameSettings;

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        SpawnObstacle();
    }

    private void Update()
    {
       // CheckPositionObstacle();
    }

    private void SpawnObstacle()
    {
        for (int obstacleZIndex = 0; obstacleZIndex < _gameSettings.obstacleCount; obstacleZIndex++)
        {

            List<int> allLaneIndices = new List<int>();
            for (int i = 0; i < _gameSettings.laneCount; i++)
            {
                allLaneIndices.Add(i);
            }
            allLaneIndices.Shuffle();

            int obstacleSpawnAmount = Random.Range(1, _gameSettings.laneCount + 1);
            
            float zRandomOffset = Random.Range(_gameSettings.zMinDifferenceBetweenObstacles, _gameSettings.zMaxDifferenceBetweenObstacles);
            // Debug.Log(zRandomOffset);
            Debug.Log(_gameSettings.lastObstaclePositionZ);
            float newZPosition = _gameSettings.lastObstaclePositionZ + zRandomOffset;
            for (int i = 0; i < obstacleSpawnAmount; i++)
            {
                float xPosition = _gameSettings.firstLanePositionX + allLaneIndices[i] * _gameSettings.distanceBetweenLanes;
                GameObject spawnedObstacle = Instantiate(_gameSettings.obstaclePrefab, new Vector3(xPosition, 0, newZPosition), Quaternion.identity);
                _gameSettings.ObstacleObjects.Add(spawnedObstacle);

                // RePositionObstacle(spawnedObstacle);

            }

           
            _gameSettings.lastObstaclePositionZ = newZPosition;
        }
    }

    private void CheckPositionObstacle()
    {
        for (int index = 0; index < _gameSettings.ObstacleObjects.Count; index++)
        {
            GameObject obstacle = _gameSettings.ObstacleObjects[index];
            if (obstacle.transform.position.z < _gameSettings.player.transform.position.z - 10f)
            {
                RePositionObstacle(obstacle);
            }
        }
    }

    private void RePositionObstacle(GameObject obstacle)
    {
   
        float zRandomOffset = Random.Range(_gameSettings.zMinDifferenceBetweenObstacles, _gameSettings.zMaxDifferenceBetweenObstacles);
        float newZPosition = _gameSettings.lastObstaclePositionZ + zRandomOffset;
        //objectpooling
        obstacle.transform.position = new Vector3(obstacle.transform.position.x, 0f,obstacle.transform.position.z+35f);
       // _gameSettings.LastObstaclePositionZ = newZPosition;
        Debug.Log(_gameSettings.lastObstaclePositionZ);
    }
}