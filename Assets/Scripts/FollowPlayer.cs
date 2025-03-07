using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{  
    public GameObject target;

    private Vector3 offset=new Vector3(0,5,-4);
    
    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
