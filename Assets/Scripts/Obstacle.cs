using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private GameSettings _gameSettings;
    void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
    }

    void Update()
    {
        if (transform.position.z < _gameSettings.player.transform.position.z - _gameSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool("Obstacle", gameObject); //   objelerini havuza döndür
        }
    }
}
