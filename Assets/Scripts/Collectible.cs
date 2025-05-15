using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private GameSettings _gameSettings;
    [SerializeField]
    private PlayerSettings _playerSettings;
    [SerializeField]
    private CollectibleSettings _collectibleSettings;
    void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>();
    }

    void Update()
    {
        if (transform.position.z < _gameSettings.player.transform.position.z - _playerSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool(CollectibleSettings.COLLECTIBLE_TAG_STRING, gameObject); //  collectible objelerini havuza döndür
        }
    }
}
