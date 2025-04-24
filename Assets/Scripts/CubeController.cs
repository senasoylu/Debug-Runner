using UnityEngine;

public class CubeController : MonoBehaviour
{
    private GameObject _target;
    private Vector3 _offset;
    private Vector3 _velocity;
    [SerializeField] private float _smoothTime = 0.15f;

    [HideInInspector] public CubeController below;

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
            transform.position, desired, ref _velocity, _smoothTime
        );
    }

    void OnTriggerEnter(Collider other)
    {
        // Sadece bu küp önce Collectible olarak toplanmışsa (CollectorCube tag’li) ve
        // çarptığı obje hâlâ Collectible tag’liyse
        if (this.CompareTag("CollectorCube") && other.CompareTag("Collectible"))
        {
            // Kendi nesnesini tekrar toplamasın
            if (other.gameObject == this.gameObject) return;

            PlayerController.Instance.CollectCube(other.gameObject);
        }
    }


}