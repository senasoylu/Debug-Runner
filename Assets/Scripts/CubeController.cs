using UnityEngine;

public class CubeController : MonoBehaviour
{
    [HideInInspector] public CubeController below;

    private GameObject _target;
    private Vector3 _offset;
    private Vector3 _velocity;
    [SerializeField] private float _smoothTime = 0.15f;

    public void SetTargetStacked(GameObject target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
    }

    void LateUpdate()
    {
        if (_target == null) return;
        Vector3 desired = _target.transform.position + _offset;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desired,
            ref _velocity,
            _smoothTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            PlayerController.Instance.OnObstacleHitByCube(this);
            return;
        }

        if (CompareTag("CollectorCube") && other.CompareTag("Collectible"))
        {
            if (other.gameObject == gameObject) return;
            CubeManager.Instance.CollectCube(other.gameObject, this);
        }
    }
}
