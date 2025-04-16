using UnityEngine;

public class Building : MonoBehaviour
{
    private GameSettings _gameSettings;
    void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
    }

    void Update()
    {
        if (transform.position.z < _gameSettings.player.transform.position.z - _gameSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool(GameSettings.BUILDING_TAG_STRING, gameObject); //   objelerini havuza döndür
        }
    }
}
