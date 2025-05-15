
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "Settings/PlatformSettings", order = 1)]

public class PlatformSettings : ScriptableObject 
{
    public float firstLanePositionX = -6.0f;
    public float distanceBetweenLanes = 5.0f;

   
    public int platformCount=2;
    public int laneCount = 5;

    public float platformLength = 100.0f;
}
