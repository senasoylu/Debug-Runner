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
        //  Hedef atanmamışsa (örneğin henüz SetTargetStacked çağrılmadıysa) çık
        if (_target == null) return;

        Vector3 current = transform.position;
        Vector3 desired = _target.transform.position + _offset;
        transform.position = Vector3.SmoothDamp(
            current, desired, ref _velocity, _smoothTime
        );
    }
}
