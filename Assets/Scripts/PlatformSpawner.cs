using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    private GameSettings _gameSettings;

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        SpawnPlatformParents();
    }

    private void SpawnPlatformParents()
    {
        for (int i = 0; i < _gameSettings.platformCount; i++)
        {
            GameObject spawnedPlatformParent = Instantiate(_gameSettings.platformParentPrefab);
            spawnedPlatformParent.transform.position = new Vector3(0, 0, spawnedPlatformParent.transform.position.z + (i * _gameSettings.platformLength));
            SpawnLanesForPlatform(spawnedPlatformParent);
            _gameSettings.platformParentObjects.Add(spawnedPlatformParent);
        }
    }

    private void Update()
    {
        EndlessPlatform();
    } 

    private void SpawnLanesForPlatform(GameObject platformObj)
    {
        for (int i = 0; i < _gameSettings.laneCount; i++)
        {
            float xPosition = (i - (_gameSettings.laneCount - 1) / 2f) * _gameSettings.distanceBetweenLines;
            Vector3 spawnPosition = new Vector3(xPosition, 0, platformObj.transform.position.z - _gameSettings.platformLength / 2);

            GameObject newLane = Instantiate(_gameSettings.laneSpawnPlatformPrefab, spawnPosition, Quaternion.identity);
            newLane.transform.SetParent(platformObj.transform);
        }
    }

    private void EndlessPlatform()
    {
        foreach (GameObject currentPlatformParent in _gameSettings.platformParentObjects)
        {
            if (_gameSettings.player.transform.position.z > currentPlatformParent.transform.position.z + _gameSettings.platformLength / 2f)
            {
                currentPlatformParent.transform.position = new Vector3(0, 0, currentPlatformParent.transform.position.z + _gameSettings.platformLength * _gameSettings.platformCount);
            }
        }
    }
}
