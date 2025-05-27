using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIController : MonoBehaviour
{
    public static UpgradeUIController Instance;
    [SerializeField] 
    private PlayerController _playerController;

    [SerializeField] 
    private GameObject _upgradePanel;
    [SerializeField] 
    private Button _speedButton;
    [SerializeField]
    private Button _scoreButton;

    private void Awake()
    {
        // Singleton eriþimi
        if (Instance == null)
            Instance = this;

        // Panel baþlangýçta kapalý olsun
        _upgradePanel.SetActive(false);

        // Butonlara týklanýnca ne olacak?
        _speedButton.onClick.AddListener(ChooseSpeed);
        _scoreButton.onClick.AddListener(ChooseScore);
    }

    public void ShowPanel()
    {
        _upgradePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ChooseSpeed()
    {

        Debug.Log("Hýz x2 seçildi");
        _playerController.MultiplySpeed(2f);
        StartCoroutine(ResetSpeedAfterDelay(10f, 0.5f));
        ResumeGame();
    }
    private System.Collections.IEnumerator ResetSpeedAfterDelay(float delay, float multiplier)
    {
        yield return new WaitForSecondsRealtime(delay); // zaman dursa bile iþler
      _playerController.MultiplySpeed(multiplier);
    }
    private void ChooseScore()
    {
        Debug.Log("Puan x2 seçildi");

        GameManager.Instance.SetScoreMultiplier(2f);
        StartCoroutine(ResetScoreMultiplierAfterDelay(10f));

        ResumeGame();
    }
    private System.Collections.IEnumerator ResetScoreMultiplierAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Debug.Log("Puan çarpaný normale döndü.");
        GameManager.Instance.SetScoreMultiplier(1f);
    }

    private void ResumeGame()
    {
        _upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
