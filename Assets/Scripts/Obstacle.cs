using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private ObstacleSettings _obstacleSettings;

    private Transform _playerTransform;
    void Update()
    {
        if (transform.position.z < _playerTransform.position.z -_obstacleSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool(ObstacleSettings.OBSTACLE_TAG_STRING, gameObject); 
        }
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
