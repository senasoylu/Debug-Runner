using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private GameSettings gameSettings;


    void Start()
    {
        gameSettings = FindObjectOfType<GameSettings>();
        EnemySpawn();



    }
    private void Update()
    {

        MoveObstacle();

    }
    private void EnemySpawn()
    {

        for (int i = 0; i < gameSettings.obstacleCount; i++)
        {

            int selectedLaneIndex = Random.Range(0, gameSettings.laneCount); // þerit aralýðý
            float xposition = gameSettings.firstLanePositionX + selectedLaneIndex * gameSettings.distanceBetweenLines;


            GameObject spawnedPlatformParent = Instantiate(gameSettings.obstaclePrefab);

            float zDifferenceBetweenObstacle = Random.Range(9f, 15f);
            spawnedPlatformParent.transform.position = new Vector3(xposition, 0, spawnedPlatformParent.transform.position.z + (i * zDifferenceBetweenObstacle));

            gameSettings.ObstacleObjects.Add(spawnedPlatformParent);
            gameSettings.LastObstaclePositionZ = spawnedPlatformParent.transform.position.z;


        }


    }
    void MoveObstacle()
    {
        foreach (GameObject obstacle in gameSettings.ObstacleObjects)
        {
            if (obstacle.transform.position.z < gameSettings.player.transform.position.z - 20f)
            {

                float zDifferenceBetweenObstacle = Random.Range(9f, 15f); 

                float newZ = gameSettings.LastObstaclePositionZ + zDifferenceBetweenObstacle;
                int selectedLaneIndex = Random.Range(0, gameSettings.laneCount); // þerit aralýðý
                float xposition = gameSettings.firstLanePositionX + selectedLaneIndex * gameSettings.distanceBetweenLines;
                obstacle.transform.position = new Vector3(xposition, 0, newZ);
                gameSettings.LastObstaclePositionZ = newZ;

            }

        }

    }




}
