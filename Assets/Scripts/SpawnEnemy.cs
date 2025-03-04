using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private GameSettings _gameSettings;

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        EnemySpawn();
    }

    private void Update()
    {
        MoveObstacle();
    }

    private void EnemySpawn()
    {
        for (int i = 0; i < _gameSettings.obstacleCount; i++)
        {
            int selectedLaneIndex = Random.Range(0, _gameSettings.laneCount); // þerit aralýðý

            float xposition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes;

            GameObject spawnedPlatformParent = Instantiate(_gameSettings.obstaclePrefab);

            float zDifferenceBetweenObstacle = Random.Range(9f, 15f);
            spawnedPlatformParent.transform.position = new Vector3(xposition, 0, spawnedPlatformParent.transform.position.z + (i * zDifferenceBetweenObstacle));

            _gameSettings.ObstacleObjects.Add(spawnedPlatformParent);
            _gameSettings.LastObstaclePositionZ = spawnedPlatformParent.transform.position.z;
        }
    }

    private void MoveObstacle()
    {
        foreach (GameObject obstacle in _gameSettings.ObstacleObjects)
        {
            if (obstacle.transform.position.z < _gameSettings.player.transform.position.z - 20f)
            {
                float zDifferenceBetweenObstacle = Random.Range(9f, 15f);
                float newZ = _gameSettings.LastObstaclePositionZ + zDifferenceBetweenObstacle;
               

                int selectedLaneIndex = Random.Range(0, _gameSettings.laneCount); // þerit aralýðý

                float xposition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes;

                obstacle.transform.position = new Vector3(xposition, 0, newZ);
                _gameSettings.LastObstaclePositionZ = newZ;
            }
        }
    }
}
