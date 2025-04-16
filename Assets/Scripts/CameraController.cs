using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    private Vector3 offset = new Vector3(0, 5, -4);

    [SerializeField]
    private float _smoothTime = 0.1f;

    private Vector3 _velocity;


    private void Start()
    {
        transform.position = target.transform.position + offset;
    }

    void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 desiredPosition = target.transform.position + offset;

        transform.position = Vector3.SmoothDamp(currentPosition, desiredPosition, ref _velocity, _smoothTime);


    }
}
