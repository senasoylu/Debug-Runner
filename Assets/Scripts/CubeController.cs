using UnityEngine;

public class CubeController : MonoBehaviour
{
    [HideInInspector] public CubeController below;

    private GameObject _target;
    private Vector3 _offset;
    private Vector3 _velocity;
    [SerializeField] private float _smoothTime = 0.15f;

    /// <summary>
    /// Yığına eklendiğinde takip edilecek hedef ve ofseti ayarlar.
    /// </summary>
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

    void OnTriggerEnter(Collider other)
    {
        // 1) Eğer bir obstacle ile çarptıysa:
        if (other.CompareTag("Obstacle"))
        {
            // Kendi index’i ve sonrasını kopar
            PlayerController.Instance.OnObstacleHitByCube(this);
            return;
        }

        // 2) Eğer yığındaki bir küp, yeni collectible ile çarpıştıysa:
        if (CompareTag("CollectorCube") && other.CompareTag("Collectible"))
        {
            if (other.gameObject == gameObject) return;
            PlayerController.Instance.CollectCube(other.gameObject);
        }
    }
}
