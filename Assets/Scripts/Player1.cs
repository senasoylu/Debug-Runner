using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update



    private GameSettings gameSettings;
    private GameManager gameManager;

    public bool canMove = true;
    void Start()
    {
        gameSettings = FindObjectOfType<GameSettings>();
        gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {


        if (!gameManager.gameStarted || !canMove) return;
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
        if (Input.GetKeyDown(KeyCode.D) && gameSettings.currentLaneIndex < gameSettings.laneCount - 1) //geçtiðim index þerit indexinden küçükse bir saða d ye basýnca
        {
            gameSettings.currentLaneIndex++;
        }
        if (Input.GetKeyDown(KeyCode.A) && gameSettings.currentLaneIndex > 0) //geçtiðim index sýfýrdan büyükse bir sola a ya basýnca
        {
            gameSettings.currentLaneIndex--;
        }
    }

    private void MoveForward()  //bura
    {
        transform.Translate(Vector3.forward * Time.deltaTime * gameSettings.playerForwardSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {

            canMove = false;
            gameManager.ShowGameOver();

        }
        else if (other.CompareTag("Collectible"))
        {
            gameManager.AddScore(10);
            other.gameObject.SetActive(false);


        }
    }


}
