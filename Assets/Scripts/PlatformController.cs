using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private PlatformSettings _platformSettings;

    [SerializeField]
    private List<GameObject> _platformParentObjects = new List<GameObject>();

    private void Start()
    {
        SpawnPlatformParents();
    }
    
    public PlatformSettings GetPlatformsSettings()
    { 
        return _platformSettings;
    }
    private void SpawnPlatformParents()
    {
        for (int i = 0; i < _platformSettings.platformCount; i++)
        {
            GameObject spawnedPlatformParent = Instantiate(_platformSettings.platformParentPrefab);
            spawnedPlatformParent.transform.position = new Vector3(0, 0, spawnedPlatformParent.transform.position.z + (i * _platformSettings.platformLength));
            SpawnLanesForPlatform(spawnedPlatformParent);
           _platformParentObjects.Add(spawnedPlatformParent);
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
          
            Vector3 spawnPosition = new Vector3(xPosition, 0, platformObj.transform.position.z - _platformSettings.platformLength / _platformSettings.halfOfPlatformWidth);

            GameObject newLane = Instantiate(_platformSettings.laneSpawnPlatformPrefab, spawnPosition, Quaternion.identity);
            newLane.transform.SetParent(platformObj.transform);
        }
    }

    private void MovePlatformsIfNeeded()
    {
        foreach (GameObject currentPlatformParent in _platformParentObjects)
        {
            if (_playerController.transform.position.z > currentPlatformParent.transform.position.z + _platformSettings.platformLength /_platformSettings.halfOfPlatformWidth)
            {
                currentPlatformParent.transform.position = new Vector3(0, 0, currentPlatformParent.transform.position.z + _platformSettings.platformLength * _platformSettings.platformCount);
            }
        }
    }
}
