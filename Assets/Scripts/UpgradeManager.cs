using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] 
    private Transform player;

    [SerializeField] 
    private float upgradeTriggerZ = 100f;

    [SerializeField]
    private float nextUpgradeTriggerZ = 100f;

    [SerializeField] 
    private UpgradeUIController _upgradeUIController;

    private void Update()
    {
        if (player == null)
        { 
            return; 
        }

        if (player.position.z >= nextUpgradeTriggerZ)
        {
            _upgradeUIController.ShowPanel();
            nextUpgradeTriggerZ += upgradeTriggerZ;
        }
    }
}
