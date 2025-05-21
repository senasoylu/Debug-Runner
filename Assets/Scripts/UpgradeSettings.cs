using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UpgradeData", menuName = "Settings/UpgradeSettings", order = 1)]
public class UpgradeSettings : ScriptableObject
{
    public string upgradeName;
    public Sprite icon;

    public enum UpgradeType
    {
        SpeedMultiplier,
        ScoreMultiplier
    }

    public UpgradeType type;
    public float multiplier = 2f;
}
