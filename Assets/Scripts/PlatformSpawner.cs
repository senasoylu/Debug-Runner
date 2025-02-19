using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
private GameSettings gameSettings;
    

    void Start()
    {
        
        gameSettings=FindObjectOfType<GameSettings>();
        SpawnPlatformParents();
    }

    private void SpawnPlatformParents()
    {
        for (int i = 0; i < gameSettings.platformCount; i++)
        {
            GameObject spawnedPlatformParent = Instantiate(gameSettings.platformParentPrefab);
            spawnedPlatformParent.transform.position = new Vector3(0, 0, spawnedPlatformParent.transform.position.z + (i * gameSettings.platformLength));
            SpawnLanesForPlatform(spawnedPlatformParent);
            gameSettings.platformParentObjects.Add(spawnedPlatformParent);

        }
    }

    // Update is called once per frame
    void Update()
    {
        EndlessPlatform();
      
    }
    private void SpawnLanesForPlatform(GameObject platformObj)
    {

        for (int i = 0; i < gameSettings.laneCount; i++)
        {

            float xPosition = (i - (gameSettings.laneCount - 1) / 2f) * gameSettings.distanceBetweenLines;
            Vector3 spawnPosition = new Vector3(xPosition, 0, platformObj.transform.position.z - gameSettings.platformLength / 2);

            GameObject newLane = Instantiate(gameSettings.laneSpawnPlatformPrefab, spawnPosition, Quaternion.identity);
            newLane.transform.SetParent(platformObj.transform);

        }
    }
    private void EndlessPlatform()
    {
        foreach (GameObject currentPlatformParent in gameSettings.platformParentObjects)
        {
            if (gameSettings.player.transform.position.z > currentPlatformParent.transform.position.z + gameSettings.platformLength / 2f)
            {
                currentPlatformParent.transform.position = new Vector3(0, 0, currentPlatformParent.transform.position.z + gameSettings.platformLength * gameSettings.platformCount);

            }
        }
    }
}
