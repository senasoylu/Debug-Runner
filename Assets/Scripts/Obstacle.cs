using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameSettings _gameSettings;
    [SerializeField]
    private PlayerSettings _playerSettings;
    [SerializeField]
    private ObstacleSettings _obstacleSettings;
    void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
    }

    void Update()
    {
        if (transform.position.z < _gameSettings.player.transform.position.z - _gameSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool(ObstacleSettings.OBSTACLE_TAG_STRING, gameObject); 
        }
    }
}
