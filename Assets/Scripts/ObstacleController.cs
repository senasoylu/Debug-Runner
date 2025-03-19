using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private GameSettings _gameSettings;
    public static ObstacleController Instance { get; private set; }

    private void Awake()
    {
        Instance = this; // Singleton ataması
    }

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        SpawnObstacle();
    }

    private void Update()
    {
        CheckPositionObstacle();
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
            float newZPosition = _gameSettings.lastObstaclePositionZ + zRandomOffset;

            for (int i = 0; i < obstacleSpawnAmount; i++)
            {
                float xPosition = _gameSettings.firstLanePositionX + allLaneIndices[i] * _gameSettings.distanceBetweenLanes;

                // DİKKAT: PoolManager'da "Obstacle" tag'i olduğundan emin olun (büyük harf "O").
                GameObject spawnedObstacle = PoolManager.Instance.GetFromPool("Obstacle");

                _gameSettings.ObstacleObjects.Add(spawnedObstacle);
                RePositionObstacle(spawnedObstacle);
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

        // Engelin konumunu basitçe z ekseninde 35 birim ileriye taşıyorsunuz (veya newZPosition'a ayarlayabilirsiniz).
        obstacle.transform.position = new Vector3(obstacle.transform.position.x, 0f, obstacle.transform.position.z + 35f);
        // _gameSettings.lastObstaclePositionZ = newZPosition; // İsteğe bağlı
    }

    public void ReturnObstacle(GameObject obstacle)
    {
        // OBSTACLE listesi üzerinden çıkarma (CollectibleObjects değil!)
        if (_gameSettings.ObstacleObjects.Contains(obstacle))
        {
            _gameSettings.ObstacleObjects.Remove(obstacle);
        }

        // Tag da "Obstacle" (büyük O) ile uyuşmalı
        PoolManager.Instance.ReturnToPool("Obstacle", obstacle);

       // StartCoroutine(ReturnAfterTime());
    }

   
}
