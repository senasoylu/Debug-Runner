using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Settings/PlayerSettings", order = 1)]

public class PlayerSettings : ScriptableObject
{
    public float playerForwardSpeed = 10.0f;
    public float playerSidewaySpeed = 20.0f;
    public float distanceMovingToPlayer = 20f; ///
    public float jumpDuration = 1f;
    public float jumpHeight = 3f;

    public int currentLaneIndex = 1;

}
