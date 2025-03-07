using UnityEngine;

public class Collectible : MonoBehaviour
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

    private void SpawnCollectible()
    {
        int laneCountWithConnections = _gameSettings.laneCount + _gameSettings.laneCount - 1;
        _lastSelectedLaneIndex = Random.Range(0, laneCountWithConnections);

        for (int i = 0; i < _gameSettings.collectibleCount; i++)
        {
            int selectedLaneIndex;
            if (_lastSelectedLaneIndex > 0 && _lastSelectedLaneIndex < laneCountWithConnections - 1)
            {
                selectedLaneIndex = Random.Range(_lastSelectedLaneIndex - 1, _lastSelectedLaneIndex + 2);
            }
            else
            {
                selectedLaneIndex = Random.Range(0, laneCountWithConnections);
            }
            _lastSelectedLaneIndex = selectedLaneIndex;

            float xPosition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes / 2;


            float zDifferenceBetweenCollectibles = Random.Range(2f, 5f);

            GameObject spawnedCollectibleParent = Instantiate(_gameSettings.collectiblePrefab);
            spawnedCollectibleParent.transform.position = new Vector3(xPosition, 0.5f, spawnedCollectibleParent.transform.position.z + (i * zDifferenceBetweenCollectibles));

            _gameSettings.CollectibleObjects.Add(spawnedCollectibleParent);
            _gameSettings.lastCollectiblePositionZ = spawnedCollectibleParent.transform.position.z;
        }
    }

    private void MoveCollectibles()
    {

         _lastSelectedLaneIndex = 0;
        for (int index = 0; index < _gameSettings.CollectibleObjects.Count; index++)
        {
            GameObject collectible = _gameSettings.CollectibleObjects[index];
            MoveCollectible(index, collectible);
        }
    }

    private void MoveCollectible(int index, GameObject collectible)
    {
        if (_gameSettings.CollectibleObjects[index].transform.position.z < _gameSettings.player.transform.position.z - 20f)
        {
            collectible.gameObject.SetActive(true);
            float zDifferenceBetweenCollectibles = Random.Range(2f, 5f);

            float newZ = _gameSettings.lastCollectiblePositionZ + zDifferenceBetweenCollectibles;
            int selectedLaneIndex = Random.Range(0, _gameSettings.laneCount); // þerit aralýðý
            _lastSelectedLaneIndex = selectedLaneIndex;
            if (_lastSelectedLaneIndex > 0 && _lastSelectedLaneIndex < _gameSettings.laneCount - 1)
            {
                selectedLaneIndex = Random.Range(_lastSelectedLaneIndex - 1, _lastSelectedLaneIndex + 2);
            }

            else
            {
                selectedLaneIndex = Random.Range(0, _gameSettings.laneCount);
            }

            float xposition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes;
            collectible.transform.position = new Vector3(xposition, 0.5f, newZ);
            _gameSettings.lastCollectiblePositionZ = newZ;
        }
    }
}
// eðer benim lastselectedelaneýndexým+1 < game.settings.lanecount ve lastselectedlaneýndex-1>0  
//selectedlaneIndex= randomrange(lastselectedýndex-1, lastselected+1) 
