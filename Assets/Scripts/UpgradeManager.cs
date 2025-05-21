using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] 
    private Transform player;
    [SerializeField] 
    private GameObject upgradeZonePrefab;
    [SerializeField]
    private float spawnDistance = 100f;

    private float _lastSpawnZ = 0f;

    private void Update()
    {
        if (player == null || upgradeZonePrefab == null) return;

        if (player.position.z > _lastSpawnZ + spawnDistance)
        {
            Vector3 spawnPos = new Vector3(0f, 0f, player.position.z + 30f);
            Instantiate(upgradeZonePrefab, spawnPos, Quaternion.identity);
            _lastSpawnZ = player.position.z;
        }
    }
}
