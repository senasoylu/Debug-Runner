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
            bool useSecondPrefab = Random.value > 0.5f;
            GameObject chosenPrefab = useSecondPrefab
                ? _gameSettings.obstaclePrefab2
                : _gameSettings.obstaclePrefab;

            GameObject spawnedObstacle = Instantiate(chosenPrefab);

            if (useSecondPrefab)
            {
                _gameSettings.ObstacleObjects2.Add(spawnedObstacle);
            }
            else
            {
                _gameSettings.ObstacleObjects.Add(spawnedObstacle);
            }

            RePositionObstacle(spawnedObstacle,useSecondPrefab);
        }
    } 

    private void CheckPositionObstacle()
    {
        for (int index = 0; index < _gameSettings.ObstacleObjects.Count; index++)
        {
            GameObject obstacle = _gameSettings.ObstacleObjects[index];
            if (obstacle.transform.position.z < _gameSettings.player.transform.position.z - 20f)
            {
                RePositionObstacle(obstacle,false);
            }
        }
        for (int j = 0; j < _gameSettings.ObstacleObjects2.Count; j++)
        {
            GameObject obstacle2 = _gameSettings.ObstacleObjects2[j];
            if (obstacle2.transform.position.z < _gameSettings.player.transform.position.z - 20f)
            {
                RePositionObstacle(obstacle2,true);
            }
        }
    }

    private void RePositionObstacle(GameObject obstacle, bool isSecondPrefab)
    {
        // 1) Hangi şeritler seçilebilir?
        int selectedLaneIndex;
        if (isSecondPrefab)
        {
            // Büyük engel ise, uç şeritlerden (0 ve laneCount-1) hariç tut:
            // Örneğin, laneCount = 5 → şeritler: 0,1,2,3,4
            // Yalnızca 1,2,3 arasından seçmek için:
            selectedLaneIndex = Random.Range(1, _gameSettings.laneCount - 1);
        }
        else
        {
            // Küçük engel ise, tüm şeritlerden seç
            selectedLaneIndex = Random.Range(0, _gameSettings.laneCount);
        }

        // 2) x konumunu hesapla
        float xPosition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes;

        // 3) z konumu (Random aralık)
        float zRandomOffset = Random.Range(9f, 15f);
        float newZPosition = _gameSettings.LastObstaclePositionZ + zRandomOffset;

        // 4) Konumlandırma
        obstacle.transform.position = new Vector3(xPosition, 0f, newZPosition);

        // 5) Son engel konumunu güncelle (tüm engelleri aynı z sıralamasında takip ettiğinizi varsayarsak)
        _gameSettings.LastObstaclePositionZ = obstacle.transform.position.z;
    }
}
