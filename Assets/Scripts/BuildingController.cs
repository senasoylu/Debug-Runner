using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private GameSettings _gameSettings;
    public GameObject player;
    public float lastBuildingPositionZLeft;
    public float lastBuildingPositionZRight;

    void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        lastBuildingPositionZLeft = _gameSettings.player.transform.position.z + _gameSettings.distanceMovingToPlayer;
        lastBuildingPositionZRight = _gameSettings.player.transform.position.z + _gameSettings.distanceMovingToPlayer;
    }

    void Update()
    {
        SpawnLeftBuildings();
        SpawnRightBuildings();
    }

    public void SpawnLeftBuildings()
    {
        while (lastBuildingPositionZLeft < _gameSettings.player.transform.position.z + 150f)
        {
            float xLeftPosition = Random.Range(_gameSettings.minOnTheLeftSide, _gameSettings.maxOnTheLeftSide);
            float zRandomOffset = Random.Range(_gameSettings.minDifferenceBetweenBuilding, _gameSettings.maxDifferenceBetweenBuilding);
            float newZPosition = lastBuildingPositionZLeft + zRandomOffset;
            float yPosition = Random.Range(_gameSettings.minHeight, _gameSettings.maxHeight);

            GameObject leftSideBuilding = PoolManager.Instance.GetFromPool(GameSettings.BUILDING_TAG_STRING);
           
            leftSideBuilding.transform.position = new Vector3(xLeftPosition, yPosition, newZPosition);
            lastBuildingPositionZLeft = newZPosition;
        }
    }

    public void SpawnRightBuildings()
    {
        while (lastBuildingPositionZRight < _gameSettings.player.transform.position.z + 150f)
        {
            float xRightPosition = Random.Range(_gameSettings.minOnTheRightSide, _gameSettings.maxOnTheRightSide);
            float yPosition = Random.Range(_gameSettings.minHeight, _gameSettings.maxHeight);
            float zRandomOffset = Random.Range(_gameSettings.minDifferenceBetweenBuilding, _gameSettings.maxDifferenceBetweenBuilding);
            float newZPosition = lastBuildingPositionZRight + zRandomOffset;

            GameObject RightSideBuilding = PoolManager.Instance.GetFromPool(GameSettings.BUILDING_TAG_STRING);

            RightSideBuilding.transform.position = new Vector3(xRightPosition, yPosition, newZPosition);
            lastBuildingPositionZRight = newZPosition;
        }
    }
}
