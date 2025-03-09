using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{  
    public GameObject target;

    private Vector3 offset=new Vector3(0,5,-4);
    
    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}
