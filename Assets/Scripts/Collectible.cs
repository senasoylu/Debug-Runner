using UnityEngine;

public class Collectible : MonoBehaviour
{
    private GameSettings _gameSettings;

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        SpawnCollectible();
    }

    private void Update()
    {
        MoveCollectible();
    }

    private void SpawnCollectible()
    {
        for (int i = 0; i < _gameSettings.collectibleCount; i++)
        {
            int selectedLaneIndex = Random.Range(0, _gameSettings.laneCount);
            float xPosition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes;
            float zDifferenceBetweenCollectibles = Random.Range(5f, 8f);

            GameObject spawnedCollectibleParent = Instantiate(_gameSettings.collectiblePrefab);
            spawnedCollectibleParent.transform.position = new Vector3(xPosition, 0, spawnedCollectibleParent.transform.position.z + (i * zDifferenceBetweenCollectibles));

            _gameSettings.CollectibleObjects.Add(spawnedCollectibleParent);
            _gameSettings.lastCollectiblePositionZ = spawnedCollectibleParent.transform.position.z;
        }
    }

    private void MoveCollectible()
    {
        foreach (GameObject collectible in _gameSettings.CollectibleObjects)
        {
            if (collectible.transform.position.z < _gameSettings.player.transform.position.z - 20f)
            {
                collectible.gameObject.SetActive(true);
                float zDifferenceBetweenCollectibles = Random.Range(5f, 8f);

                float newZ = _gameSettings.lastCollectiblePositionZ + zDifferenceBetweenCollectibles;
                int selectedLaneIndex = Random.Range(0, _gameSettings.laneCount); // þerit aralýðý
                float xposition = _gameSettings.firstLanePositionX + selectedLaneIndex * _gameSettings.distanceBetweenLanes;

                collectible.transform.position = new Vector3(xposition, 0, newZ);
                _gameSettings.lastCollectiblePositionZ = newZ;
            }
        }
    }
}
