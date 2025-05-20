using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    private float _lastBuildingPositionZLeft;
    private float _lastBuildingPositionZRight;

    [SerializeField]
    private EnviromentSettings _enviromentSettings;
 
    void Start()
    {
        _lastBuildingPositionZLeft = _playerController.transform.position.z + _enviromentSettings.distanceMovingToPlayer;
        _lastBuildingPositionZRight =_playerController.transform.position.z + _enviromentSettings.distanceMovingToPlayer;
    }

    void Update()
    {
        SpawnLeftBuildings();
        SpawnRightBuildings();
    }

    public void SpawnLeftBuildings()
    {
        while (_lastBuildingPositionZLeft < _playerController.transform.position.z +_enviromentSettings._lastBuildingPositionZTop )
        {
            float xLeftPosition = Random.Range(_enviromentSettings.minOnTheLeftSide, _enviromentSettings.maxOnTheLeftSide);
            float zRandomOffset = Random.Range(_enviromentSettings.minDifferenceBetweenBuilding, _enviromentSettings.maxDifferenceBetweenBuilding);
            float newZPosition = _lastBuildingPositionZLeft + zRandomOffset;
            float yPosition = Random.Range(_enviromentSettings.minHeight, _enviromentSettings.maxHeight);

            GameObject leftSideBuilding = PoolManager.Instance.GetFromPool(EnviromentSettings.BUILDING_TAG_STRING);
            leftSideBuilding.GetComponent<Building>().SetPlayerTransform(transform);
           
            leftSideBuilding.transform.position = new Vector3(xLeftPosition, yPosition, newZPosition);
            _lastBuildingPositionZLeft = newZPosition;
        }
    }

    public void SpawnRightBuildings()
    {
        while (_lastBuildingPositionZRight < _playerController.transform.position.z + _enviromentSettings._lastBuildingPositionZTop)
        {
            float xRightPosition = Random.Range(_enviromentSettings.minOnTheRightSide, _enviromentSettings.maxOnTheRightSide);
            float yPosition = Random.Range(_enviromentSettings.minHeight, _enviromentSettings.maxHeight);
            float zRandomOffset = Random.Range(_enviromentSettings.minDifferenceBetweenBuilding, _enviromentSettings.maxDifferenceBetweenBuilding);
            float newZPosition = _lastBuildingPositionZRight + zRandomOffset;

            GameObject rightSideBuilding = PoolManager.Instance.GetFromPool(EnviromentSettings.BUILDING_TAG_STRING);
            rightSideBuilding.GetComponent<Building>().SetPlayerTransform(transform);

            rightSideBuilding.transform.position = new Vector3(xRightPosition, yPosition, newZPosition);
            _lastBuildingPositionZRight = newZPosition;
        }
    }
}
