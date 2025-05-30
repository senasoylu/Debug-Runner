using UnityEngine;

public class BuildingController : MonoBehaviour
{

    [SerializeField]
    private PlayerNavigationData _playerNavigationData;

    private float _lastBuildingPositionZLeft;
    private float _lastBuildingPositionZRight;

    [SerializeField]
    private EnviromentSettings _enviromentSettings;

    void Start()
    {
        _lastBuildingPositionZLeft = _playerNavigationData.GetPlayerPosition().z + _enviromentSettings.distanceMovingToPlayer;
        _lastBuildingPositionZRight =_playerNavigationData.GetPlayerPosition().z + _enviromentSettings.distanceMovingToPlayer;
    }

    void Update()
    {
        SpawnLeftBuildings();
        SpawnRightBuildings();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(EnviromentSettings.BUILDING_TAG_STRING))
        {
            if (obj.transform.position.z < _playerNavigationData.GetPlayerPosition().z - _enviromentSettings.distanceMovingToPlayer)
            {
                PoolManager.Instance.ReturnToPool(EnviromentSettings.BUILDING_TAG_STRING, obj);
            }
        }
    }

    public void SpawnLeftBuildings()
    {
        while (_lastBuildingPositionZLeft < _playerNavigationData.GetPlayerPosition().z +_enviromentSettings.lastBuildingPositionZTop )
        {
            float xLeftPosition = Random.Range(_enviromentSettings.minOnTheLeftSide, _enviromentSettings.maxOnTheLeftSide);
            float zRandomOffset = Random.Range(_enviromentSettings.minDifferenceBetweenBuilding, _enviromentSettings.maxDifferenceBetweenBuilding);
            float newZPosition = _lastBuildingPositionZLeft + zRandomOffset;
            float yPosition = Random.Range(_enviromentSettings.minHeight, _enviromentSettings.maxHeight);

            GameObject leftSideBuilding = PoolManager.Instance.GetFromPool(EnviromentSettings.BUILDING_TAG_STRING);
          
            leftSideBuilding.transform.position = new Vector3(xLeftPosition, yPosition, newZPosition);
            _lastBuildingPositionZLeft = newZPosition;
        }
    }

    public void SpawnRightBuildings()
    {
        while (_lastBuildingPositionZRight < _playerNavigationData.GetPlayerPosition().z + _enviromentSettings.lastBuildingPositionZTop)
        {
            float xRightPosition = Random.Range(_enviromentSettings.minOnTheRightSide, _enviromentSettings.maxOnTheRightSide);
            float yPosition = Random.Range(_enviromentSettings.minHeight, _enviromentSettings.maxHeight);
            float zRandomOffset = Random.Range(_enviromentSettings.minDifferenceBetweenBuilding, _enviromentSettings.maxDifferenceBetweenBuilding);
            float newZPosition = _lastBuildingPositionZRight + zRandomOffset;

            GameObject rightSideBuilding = PoolManager.Instance.GetFromPool(EnviromentSettings.BUILDING_TAG_STRING);
       
            rightSideBuilding.transform.position = new Vector3(xRightPosition, yPosition, newZPosition);
            _lastBuildingPositionZRight = newZPosition;
        }
    }
}
