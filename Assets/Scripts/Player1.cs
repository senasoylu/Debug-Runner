using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody playerRb;
    public float playerSpeed = 2.0f;
    public float maxX = 10.0f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {





    }
    private void FixedUpdate()
    {
       // playerRb.velocity = new Vector3(0, playerRb.velocity.y, playerSpeed);
       transform.Translate(Vector3.forward*Time.deltaTime*playerSpeed);
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerSpeed);

        if (transform.position.x < -maxX)
        {
            transform.position = new Vector3(-maxX, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
    }


}
