using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNavigationData", menuName = "Settings/PlayerNavigationData", order = 1)]
public class PlayerNavigationData : ScriptableObject
{
    private Vector3 _playerPosition;

    public void Subscribe()
    {
        Debug.Log("ASSAS");
        PlayerController.OnPositionChangeEvent += OnPositionChange;
    }

    private void OnPositionChange(Vector3 vector)
    {
        _playerPosition= vector;
    }

    public Vector3 GetPlayerPosition()
    {
        return _playerPosition;
    }

    public void Unsubscribe()
    {
        Debug.Log("ahhaha");
        PlayerController.OnPositionChangeEvent -= OnPositionChange;
    }
}
