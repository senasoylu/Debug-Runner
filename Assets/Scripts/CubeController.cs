using UnityEngine;

public class CubeController : MonoBehaviour
{
    [HideInInspector] 
    public CubeController below;

    private GameObject _target;

    private Vector3 _offset;
    private Vector3 _velocity;

    [SerializeField] 
    private float _smoothTime = 0.15f;

    [SerializeField]
    private ObstacleSettings _obstacleSettings;

    [SerializeField]
    private CollectibleSettings _collectibleSettings;

    public delegate void OnObstacleHit(CubeController cube);
    public static event OnObstacleHit OnObstacleHitEvent;


    private bool _isCollected = false;

    public void SetTargetStacked(GameObject target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
    }
    
    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }
        Vector3 desired = _target.transform.position + _offset;
        transform.position = Vector3.SmoothDamp( transform.position, desired, ref _velocity, _smoothTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ObstacleSettings.OBSTACLE_TAG_STRING))
        {
            OnObstacleHitEvent?.Invoke(this);
            return;
        }

        if (CompareTag(CollectibleSettings.COLLECTOR_TAG_STRING) && other.CompareTag(CollectibleSettings.COLLECTIBLE_TAG_STRING))
        {
            if (other.gameObject == gameObject) return;

            CubeManager.Instance.CollectCube(other.gameObject, this);
            return;
        }

        if (!_isCollected && other.CompareTag("Player"))
        {
            _isCollected = true;
            CubeManager.Instance.CollectCube(gameObject, this);

            PlayerController.OnCollectibleHitEvent?.Invoke();
        }
    }
}
