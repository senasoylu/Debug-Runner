using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private GameSettings _gameSettings;
    public float lastObstaclePositionZ;
    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
      lastObstaclePositionZ = _gameSettings.player.transform.position.z + _gameSettings.distanceMovingToPlayer;
        SpawnObstacle();
    }

    private void Update()
    {
        if (lastObstaclePositionZ< _gameSettings.player.transform.position.z + 150f)
        {
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        while (lastObstaclePositionZ < _gameSettings.player.transform.position.z + 150f) //oyuncunun 150 metre ilerisine kadar obstacle olsun 
        {
            List<int> allLaneIndices = new List<int>(); //lane index list
            for (int i = 0; i < _gameSettings.laneCount; i++) 
            {
                allLaneIndices.Add(i); //lane listesine indeksleri ekle
            }
            allLaneIndices.Shuffle(); //karıştır(fatih abinin gönderdiği yerden Shuffle çalıştı

            int obstacleSpawnAmount = Random.Range(1, _gameSettings.laneCount + 1); //engel sayısı değişken olsun 1 ve 5 arası olsun +1 dedik hepsi dahil olsun diye
            float zRandomOffset = Random.Range(_gameSettings.zMinDifferenceBetweenObstacles, _gameSettings.zMaxDifferenceBetweenObstacles);//obs arası mesafe
            float newZPosition = lastObstaclePositionZ + zRandomOffset; 

            for (int i = 0; i < obstacleSpawnAmount; i++)// lanelerde
            {
                float xPosition = _gameSettings.firstLanePositionX + allLaneIndices[i] * _gameSettings.distanceBetweenLanes;
                GameObject spawnedObstacle = PoolManager.Instance.GetFromPool("Obstacle");
                spawnedObstacle.transform.position = new Vector3(xPosition, 0, newZPosition);
            }
            lastObstaclePositionZ = newZPosition;
        }
    }

    //private void SpawnObstacle()
    //{
    //    for (int obstacleZIndex = 0; obstacleZIndex < _gameSettings.obstacleCount; obstacleZIndex++)
    //    {
    //        List<int> allLaneIndices = new List<int>();
    //        for (int i = 0; i < _gameSettings.laneCount; i++)
    //        {
    //            allLaneIndices.Add(i);
    //        }
    //        allLaneIndices.Shuffle();

    //        int obstacleSpawnAmount = Random.Range(1, _gameSettings.laneCount + 1);

    //        float zRandomOffset = Random.Range(_gameSettings.zMinDifferenceBetweenObstacles, _gameSettings.zMaxDifferenceBetweenObstacles);
    //        float newZPosition = _gameSettings.lastObstaclePositionZ + zRandomOffset;

    //        while (_gameSettings.lastObstaclePositionZ < _gameSettings.player.transform.position.z + 150f)
    //        {
    //            for (int i = 0; i < obstacleSpawnAmount; i++)
    //            {
    //                float xPosition = _gameSettings.firstLanePositionX + allLaneIndices[i] * _gameSettings.distanceBetweenLanes;
    //                GameObject spawnedObstacle = PoolManager.Instance.GetFromPool("Obstacle");
    //                spawnedObstacle.transform.position=new Vector3(xPosition, 0,newZPosition);
    //            }
    //            // Her iterasyonda yeni bir z offset hesaplanarak yeniZPosition güncelleniyor.
    //            zRandomOffset = Random.Range(_gameSettings.zMinDifferenceBetweenObstacles, _gameSettings.zMaxDifferenceBetweenObstacles);
    //            newZPosition += zRandomOffset;
    //            _gameSettings.lastObstaclePositionZ = newZPosition;
    //        }
    //    }
    //}

}
