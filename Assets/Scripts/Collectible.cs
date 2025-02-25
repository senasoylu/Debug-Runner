using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    GameSettings gameSettings;
    void Start()
    {
        gameSettings = FindObjectOfType<GameSettings>();
        SpawnCollectible();

    }

    // Update is called once per frame
    void Update()
    {
        MoveCollectible();
    }
    public void SpawnCollectible()
    {
        for (int i = 0; i < gameSettings.collectibleCount; i++)
        {
            int selectedLaneIndex = Random.Range(0, gameSettings.laneCount);
            float xPosition = gameSettings.firstLanePositionX + selectedLaneIndex * gameSettings.distanceBetweenLines;
            float zDifferenceBetweenCollectibles = Random.Range(5f, 8f);

            GameObject spawnedCollectibleParent = Instantiate(gameSettings.collectiblePrefab);
            spawnedCollectibleParent.transform.position = new Vector3(xPosition, 0, spawnedCollectibleParent.transform.position.z + (i * zDifferenceBetweenCollectibles));

            gameSettings.CollectibleObjects.Add(spawnedCollectibleParent);
            gameSettings.lastCollectiblePositionZ = spawnedCollectibleParent.transform.position.z;


        }
    }
    public void MoveCollectible()
    {
        foreach (GameObject collectible in gameSettings.CollectibleObjects)
        {
            if (collectible.transform.position.z < gameSettings.player.transform.position.z - 20f)
            {
                collectible.gameObject.SetActive(true);
                float zDifferenceBetweenCollectibles = Random.Range(5f, 8f);

                float newZ = gameSettings.lastCollectiblePositionZ + zDifferenceBetweenCollectibles;
                int selectedLaneIndex = Random.Range(0, gameSettings.laneCount); // þerit aralýðý
                float xposition = gameSettings.firstLanePositionX + selectedLaneIndex * gameSettings.distanceBetweenLines;

                collectible.transform.position = new Vector3(xposition, 0, newZ);
                gameSettings.lastCollectiblePositionZ = newZ;

            }

        }
    }
}
