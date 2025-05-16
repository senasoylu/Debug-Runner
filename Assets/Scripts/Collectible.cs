using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private CollectibleSettings _collectibleSettings;

    private Transform _playerTransform;
   
    void Update()
    {
        if (transform.position.z < PlayerController.Instance.transform.position.z - _collectibleSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool(CollectibleSettings.COLLECTIBLE_TAG_STRING, gameObject); //collectible objelerini havuza döndür
        }
    }
    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
