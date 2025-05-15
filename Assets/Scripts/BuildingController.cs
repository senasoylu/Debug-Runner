using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    public float lastBuildingPositionZLeft;
    public float lastBuildingPositionZRight;

    private float _lastBuildingPositionZTop = 150f;

    [SerializeField]
    private EnviromentSettings _enviromentSettings;
 

    void Start()
    {
      
        lastBuildingPositionZLeft = _playerController.transform.position.z + _playerController.GetPlayerSettings().distanceMovingToPlayer;
        lastBuildingPositionZRight =_playerController.transform.position.z + _playerController.GetPlayerSettings().distanceMovingToPlayer;
    }

    void Update()
    {
        SpawnLeftBuildings();
        SpawnRightBuildings();
    }

    public void SpawnLeftBuildings()
    {
        while (lastBuildingPositionZLeft < _playerController.transform.position.z +_lastBuildingPositionZTop )
        {
            float xLeftPosition = Random.Range(_enviromentSettings.minOnTheLeftSide, _enviromentSettings.maxOnTheLeftSide);
            float zRandomOffset = Random.Range(_enviromentSettings.minDifferenceBetweenBuilding, _enviromentSettings.maxDifferenceBetweenBuilding);
            float newZPosition = lastBuildingPositionZLeft + zRandomOffset;
            float yPosition = Random.Range(_enviromentSettings.minHeight, _enviromentSettings.maxHeight);

            GameObject leftSideBuilding = PoolManager.Instance.GetFromPool(EnviromentSettings.BUILDING_TAG_STRING);
           
            leftSideBuilding.transform.position = new Vector3(xLeftPosition, yPosition, newZPosition);
            lastBuildingPositionZLeft = newZPosition;
        }
    }

    public void SpawnRightBuildings()
    {
        while (lastBuildingPositionZRight < _playerController.transform.position.z + _lastBuildingPositionZTop)
        {
            float xRightPosition = Random.Range(_enviromentSettings.minOnTheRightSide, _enviromentSettings.maxOnTheRightSide);
            float yPosition = Random.Range(_enviromentSettings.minHeight, _enviromentSettings.maxHeight);
            float zRandomOffset = Random.Range(_enviromentSettings.minDifferenceBetweenBuilding, _enviromentSettings.maxDifferenceBetweenBuilding);
            float newZPosition = lastBuildingPositionZRight + zRandomOffset;

            GameObject RightSideBuilding = PoolManager.Instance.GetFromPool(EnviromentSettings.BUILDING_TAG_STRING);

            RightSideBuilding.transform.position = new Vector3(xRightPosition, yPosition, newZPosition);
            lastBuildingPositionZRight = newZPosition;
        }
    }
}
