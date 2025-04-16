using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
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
            PoolManager.Instance.ReturnToPool(GameSettings.COLLECT�BLE_TAG_STRING, gameObject); //  collectible objelerini havuza d�nd�r
        }
    }
}
