using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float upgradeTriggerZ = 100f;
    [SerializeField] private UpgradeUIController _upgradeUIController;

    private bool _upgradeTriggered = false;

    private void Update()
    {
        if (_upgradeTriggered || player == null) return;

        if (player.position.z >= upgradeTriggerZ)
        {
            _upgradeTriggered = true;

            // Oyunu durdur, UI paneli göster
            _upgradeUIController.ShowPanel();
        }
    }
}
