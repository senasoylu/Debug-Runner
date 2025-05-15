using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Settings/ObstacleSettings", order = 1)]

public class ObstacleSettings : ScriptableObject
{
    public float zMaxDifferenceBetweenObstacles = 35f;
    public float zMinDifferenceBetweenObstacles = 50f;

    public const string OBSTACLE_TAG_STRING = "Obstacle";
}
