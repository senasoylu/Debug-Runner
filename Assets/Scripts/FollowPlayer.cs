using UnityEngine;

public class FollowPlayer : MonoBehaviour
{  
    public GameObject target;

    private Vector3 offset=new Vector3(0,8,-10);
    
    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
