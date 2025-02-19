using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update
    

 
    private GameSettings gameSettings;
    void Start()
    {
        gameSettings=FindObjectOfType<GameSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();

        GetInputs();

        MoveSideways();

    }

    private void MoveSideways()
    {
        Vector3 desiredPosition;  // hedef pozisyon
        desiredPosition = new Vector3(gameSettings.firstLanePositionX + gameSettings.currentLaneIndex * gameSettings.distanceBetweenLines, transform.position.y, transform.position.z);
        Vector3 currentPosition = transform.position;
        Vector3 direction = desiredPosition - currentPosition;
        direction = Vector3.Normalize(direction);
        float frameSpeed = gameSettings.playerSidewaySpeed * Time.deltaTime;
        float distance = Vector3.Distance(currentPosition, desiredPosition);

        if (distance < frameSpeed)
        {
            frameSpeed = distance;
        }
        transform.position = currentPosition + direction * frameSpeed;

    }

    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.D) && gameSettings.currentLaneIndex < gameSettings.laneCount - 1) //ge�ti�im index �erit indexinden k���kse bir sa�a d ye bas�nca
        {
            gameSettings.currentLaneIndex++;
        }
        if (Input.GetKeyDown(KeyCode.A) && gameSettings.currentLaneIndex > 0) //ge�ti�im index s�f�rdan b�y�kse bir sola a ya bas�nca
        {
            gameSettings.currentLaneIndex--;
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * gameSettings.playerForwardSpeed);
    }

}
