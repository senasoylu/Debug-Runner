using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CollectibleController : MonoBehaviour
{

    private GameSettings _gameSettings;
    private int _lastSelectedLaneIndex;
    public float _lastCollectiblePositionZ;

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        _lastCollectiblePositionZ = _gameSettings.player.transform.position.z + _gameSettings.distanceMovingToPlayer;
        SpawnCollectibles();
    }

    private void Update()
    {
        if(_lastCollectiblePositionZ<_gameSettings.player.transform.position.z+150f)
        {
            GameObject spawnedCollectible = PoolManager.Instance.GetFromPool("Collectible");
        }
    }

    // Havuzdan collectible çekip sahneye yerleþtirir.
    public void SpawnCollectibles()
    {
        int laneCountWithConnections = _gameSettings.laneCount + _gameSettings.laneCount - 1;
        _lastSelectedLaneIndex = Random.Range(0, laneCountWithConnections);

        //  for (int i = 0; i < _gameSettings.collectibleCount; i++)
        while (_lastCollectiblePositionZ < _gameSettings.player.transform.position.z + 150f)
        {
            GameObject spawnedCollectible = PoolManager.Instance.GetFromPool("Collectible");
            SetNewPositionToCollectible(spawnedCollectible);
        }
    }

    private void SetNewPositionToCollectible(GameObject collectible)
    {
        int laneCountWithConnections = _gameSettings.laneCount + _gameSettings.laneCount - 1;
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

        float xPosition = _gameSettings.firstLanePositionX + selectedLaneIndex * (_gameSettings.distanceBetweenLanes / 2f);
        float zRandomOffset = Random.Range(_gameSettings.zMinDifferenceBetweenCollectibles, _gameSettings.zMaxDifferenceBetweenCollectibles);
        float newZposition = _lastCollectiblePositionZ + zRandomOffset;

        collectible.transform.position = new Vector3(xPosition, 0.5f, newZposition);
        _lastCollectiblePositionZ = collectible.transform.position.z;
    }
}
