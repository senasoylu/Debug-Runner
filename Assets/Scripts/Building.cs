using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private EnviromentSettings _enviromentSettings;

    private Transform _playerTransform;

    void Update()
    {
        Debug.Log(_enviromentSettings == null);
        if (transform.position.z < PlayerController.Instance.transform.position.z - _enviromentSettings.distanceMovingToPlayer)
        {
            PoolManager.Instance.ReturnToPool(EnviromentSettings.BUILDING_TAG_STRING, gameObject); //   objelerini havuza döndür
        }
    }
    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
}
