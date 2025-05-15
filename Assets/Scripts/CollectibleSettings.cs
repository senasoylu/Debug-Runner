using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleData", menuName = "Settings/CollectibleSettings", order = 1)]

public class CollectibleSettings : ScriptableObject
{
    public const string COLLECTIBLE_TAG_STRING = "Collectible";
    public const string COLLECTOR_TAG_STRING = "CollectorCube";

    public float zMaxDifferenceBetweenCollectibles = 2f;
    public float zMinDifferenceBetweenCollectibles = 5f;
  


}
