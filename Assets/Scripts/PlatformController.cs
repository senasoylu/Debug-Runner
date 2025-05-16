using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private GameSettings _gameSettings;

    [SerializeField]
    private PlatformSettings _platformSettings;

    private float _halfPlatformWidth = 2f;

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        SpawnPlatformParents();
    }
    
    private void SpawnPlatformParents()
    {
        for (int i = 0; i < _platformSettings.platformCount; i++)
        {
            GameObject spawnedPlatformParent = Instantiate(_gameSettings.platformParentPrefab);
            spawnedPlatformParent.transform.position = new Vector3(0, 0, spawnedPlatformParent.transform.position.z + (i * _platformSettings.platformLength));
            SpawnLanesForPlatform(spawnedPlatformParent);
            _gameSettings.platformParentObjects.Add(spawnedPlatformParent);
        }
    }

    private void Update()
    {
        MovePlatformsIfNeeded();
    } 

    private void SpawnLanesForPlatform(GameObject platformObj)
    {
        for (int i = 0; i < _platformSettings.laneCount; i++)
        {
            float xPosition = _platformSettings.firstLanePositionX +(i * _platformSettings.distanceBetweenLanes);
          
            Vector3 spawnPosition = new Vector3(xPosition, 0, platformObj.transform.position.z - _platformSettings.platformLength / _halfPlatformWidth);

            GameObject newLane = Instantiate(_gameSettings.laneSpawnPlatformPrefab, spawnPosition, Quaternion.identity);
            newLane.transform.SetParent(platformObj.transform);
        }
    }

    private void MovePlatformsIfNeeded()
    {
        foreach (GameObject currentPlatformParent in _gameSettings.platformParentObjects)
        {
            if (_gameSettings.player.transform.position.z > currentPlatformParent.transform.position.z + _platformSettings.platformLength / _halfPlatformWidth)
            {
                currentPlatformParent.transform.position = new Vector3(0, 0, currentPlatformParent.transform.position.z + _platformSettings.platformLength * _platformSettings.platformCount);
            }
        }
    }
}
