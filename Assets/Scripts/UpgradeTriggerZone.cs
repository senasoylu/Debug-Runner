using UnityEngine;

public class UpgradeTriggerZone : MonoBehaviour
{
  [SerializeField]
  private UpgradeSettings upgradeSettings;

  private bool _isUsed = false;

   private void OnTriggerEnter(Collider other)
   {

    if (_isUsed || !other.CompareTag("Player"))
    {
        return;
    }

    _isUsed = true;
    ApplyUpgrade(upgradeSettings);
    
    if (transform.parent != null)
    {
            Destroy(transform.parent.gameObject);
    }
      
   }


    private void ApplyUpgrade(UpgradeSettings upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeSettings.UpgradeType.SpeedMultiplier:
                PlayerController.Instance.GetPlayerSettings().playerForwardSpeed *= upgrade.multiplier;
                break;

            case UpgradeSettings.UpgradeType.ScoreMultiplier:
                GameManager.Instance.SetScoreMultiplier(upgrade.multiplier);
                break;
        }
    }
}
