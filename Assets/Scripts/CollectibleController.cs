using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private GameSettings _gameSettings;

    private int _lastSelectedLaneIndex;
    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        SpawnCollectible();
    }

    private void Update()
    {
        MoveCollectibles();
    }
    //Collectible ý spawnla ve olmasý gereken yere taþý
    private void SpawnCollectible()
    {
        int laneCountWithConnections = _gameSettings.laneCount + _gameSettings.laneCount - 1;
        _lastSelectedLaneIndex = Random.Range(0, laneCountWithConnections);

        for (int i = 0; i < _gameSettings.collectibleCount; i++)
        {
            GameObject spawnedCollectibleParent = Instantiate(_gameSettings.collectiblePrefab);
            _gameSettings.CollectibleObjects.Add(spawnedCollectibleParent);
            SetNewPositionToCollectible(spawnedCollectibleParent);
        }
    }
    //Collectible array kontro et eðer geride kalan collectible varsa onu ileriye doðru taþý
    private void MoveCollectibles()
    {
        for (int index = 0; index < _gameSettings.CollectibleObjects.Count; index++)
        {
            GameObject collectible = _gameSettings.CollectibleObjects[index];
       
            if (collectible.transform.position.z < _gameSettings.player.transform.position.z - _gameSettings.distanceMovingToPlayer)
            {
                SetNewPositionToCollectible(collectible);
            }
        }
    }
    // içeri parametre olarak gönderilen collectible ý uygun yere taþý
    private void SetNewPositionToCollectible(GameObject collectible)
    {
        int laneCountWithConnections = _gameSettings.laneCount + _gameSettings.laneCount - 1;
        bool isGoingRight = Random.value > 0.5f;

        if (_lastSelectedLaneIndex == laneCountWithConnections && isGoingRight == true)
        {
            isGoingRight = false;
        }
        if (_lastSelectedLaneIndex == 0 && isGoingRight == false)
        {
            isGoingRight = true;
        }
        int selectedLaneIndex = isGoingRight
            ? _lastSelectedLaneIndex + 1
            : _lastSelectedLaneIndex - 1;

        _lastSelectedLaneIndex = selectedLaneIndex;

        float xPosition = _gameSettings.firstLanePositionX + selectedLaneIndex * (_gameSettings.distanceBetweenLanes / 2f);

        float zRandomOffset = Random.Range(_gameSettings.zMinDifferenceBetweenCollectibles, _gameSettings.zMaxDifferenceBetweenCollectibles);
        float newZposition=_gameSettings.lastCollectiblePositionZ + zRandomOffset;

        collectible.transform.position = new Vector3(xPosition, 0.5f, newZposition);
        _gameSettings.lastCollectiblePositionZ = collectible.transform.position.z;
    }
}
