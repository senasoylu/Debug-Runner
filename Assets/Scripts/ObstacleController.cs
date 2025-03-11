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
        CheckPositionObstacle();
    }

    private void SpawnObstacle()
    {
        for (int i = 0; i < _gameSettings.obstacleCount; i++)
        {
         /*  bool useSecondPrefab = Random.value > 0.5f;
            GameObject chosenPrefab = useSecondPrefab
                ? _gameSettings.obstaclePrefab2
                : _gameSettings.obstaclePrefab; */

            GameObject spawnedPlatformParent = Instantiate(_gameSettings.obstaclePrefab);
            _gameSettings.ObstacleObjects.Add(spawnedPlatformParent);
            RePositionObstacle(spawnedPlatformParent);
        }
    } 

    private void CheckPositionObstacle()
    {
        for (int index = 0; index < _gameSettings.ObstacleObjects.Count; index++)
        {
            GameObject obstacle = _gameSettings.ObstacleObjects[index];
            if (obstacle.transform.position.z < _gameSettings.player.transform.position.z - 20f)
            {
                RePositionObstacle(obstacle);
            }
        }
    }

    private void RePositionObstacle(GameObject obstacle)
    {
        int selectedLaneIndex = Random.Range(0, _gameSettings.laneCount);
        float xPosition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes;

        float zRandomOffset = Random.Range(9f, 15f);
        float newZPosition = _gameSettings.LastObstaclePositionZ + zRandomOffset;
     
        obstacle.transform.position = new Vector3(xPosition, 0, newZPosition);
        _gameSettings.LastObstaclePositionZ = obstacle.transform.position.z;

    }
}
