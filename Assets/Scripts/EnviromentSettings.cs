using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnviromentData", menuName = "Settings/EnviromentSettings", order = 1)]

public class EnviromentSettings : ScriptableObject
{
    public const string BUILDING_TAG_STRING = "Building";

    public float minOnTheRightSide = 4.0f;
    public float maxOnTheRightSide = 25.0f;
    public float minOnTheLeftSide = -4.0f;
    public float maxOnTheLeftSide = -30.0f;
    public float minHeight = -15.0f;
    public float maxHeight = 12.0f;
    public float minDifferenceBetweenBuilding = 20.0f;
    public float maxDifferenceBetweenBuilding = 50.0f;
    public float distanceMovingToPlayer = 20f;
    public float _lastBuildingPositionZTop = 150f;
}
