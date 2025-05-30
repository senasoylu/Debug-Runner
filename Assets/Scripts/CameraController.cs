using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    private Vector3 _offset = new Vector3(0, 5, -4);

    [SerializeField]
    private float _smoothTime = 0.1f;

    private Vector3 _velocity;

    private void Start()
    {
        transform.position = target.transform.position + _offset;
    }

    void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 desiredPosition = target.transform.position + _offset;

        transform.position = Vector3.SmoothDamp(currentPosition, desiredPosition, ref _velocity, _smoothTime);
    }
}
