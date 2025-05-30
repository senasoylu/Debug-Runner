using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField]
    private PlatformSettings _platformSettings;

    [SerializeField]
    private CollectibleSettings _collectibleSettings;

    [SerializeField]
    private PlayerNavigationData _playerNavigationData;

    private int _lastSelectedLaneIndex;

    private float _lastCollectiblePositionZ;

    private void Start()
    {
        _lastCollectiblePositionZ = _playerNavigationData.GetPlayerPosition().z + _collectibleSettings.distanceMovingToPlayer;

        SpawnCollectibles();
    }

    private void Update()
    {
        if (_lastCollectiblePositionZ < _playerNavigationData.GetPlayerPosition().z + _collectibleSettings._distanceBetweenPlayerandCol)
        {
            GameObject spawnedCollectible = PoolManager.Instance.GetFromPool(CollectibleSettings.COLLECTIBLE_TAG_STRING);
            SetNewPositionToCollectible(spawnedCollectible);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(CollectibleSettings.COLLECTIBLE_TAG_STRING))
        {
            if (obj.transform.position.z < _playerNavigationData.GetPlayerPosition().z - _collectibleSettings.distanceMovingToPlayer)
            {
                PoolManager.Instance.ReturnToPool(CollectibleSettings.COLLECTIBLE_TAG_STRING, obj);
            }
        }

    }

    public void SpawnCollectibles()
    {
        int laneCountWithConnections = _platformSettings.laneCount + _platformSettings.laneCount - 1;
        _lastSelectedLaneIndex = Random.Range(0, laneCountWithConnections);

        while (_lastCollectiblePositionZ < _playerNavigationData.GetPlayerPosition().z + _collectibleSettings._distanceBetweenPlayerandCol)
        {
            GameObject spawnedCollectible = PoolManager.Instance.GetFromPool(CollectibleSettings.COLLECTIBLE_TAG_STRING);
            SetNewPositionToCollectible(spawnedCollectible);
        }
    }

    private void SetNewPositionToCollectible(GameObject collectible)
    {
        int laneCountWithConnections = _platformSettings.laneCount + _platformSettings.laneCount - 1;

        bool isGoingRight = Random.value > 0.5f;

        if (_lastSelectedLaneIndex == laneCountWithConnections - 1 && isGoingRight)
        {
            isGoingRight = false;
        }

        if (_lastSelectedLaneIndex == 0 && !isGoingRight)
        {
            isGoingRight = true;
        }

        int selectedLaneIndex = isGoingRight
            ? _lastSelectedLaneIndex + 1
            : _lastSelectedLaneIndex - 1;
        _lastSelectedLaneIndex = selectedLaneIndex;

        float xPosition = _platformSettings.firstLanePositionX + selectedLaneIndex * (_platformSettings.distanceBetweenLanes / 2f);
        float zRandomOffset = Random.Range(_collectibleSettings.zMinDifferenceBetweenCollectibles, _collectibleSettings.zMaxDifferenceBetweenCollectibles);
        float newZposition = _lastCollectiblePositionZ + zRandomOffset;

        collectible.transform.position = new Vector3(xPosition, 0.5f, newZposition);
        _lastCollectiblePositionZ = collectible.transform.position.z;

    }
}
