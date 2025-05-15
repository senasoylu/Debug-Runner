using UnityEngine;

public class Building : MonoBehaviour
{
    private GameSettings _gameSettings;
    [SerializeField]
    private PlayerSettings _playerSettings;
    [SerializeField]
    private EnviromentSettings _enviromentSettings;
    void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
        

    }

    void Update()
    {
        if (transform.position.z < _gameSettings.player.transform.position.z - _gameSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool(EnviromentSettings.BUILDING_TAG_STRING, gameObject); //   objelerini havuza döndür
        }
    }
}
