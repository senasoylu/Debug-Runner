using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIController : MonoBehaviour
{
    public static UpgradeUIController Instance;
    [SerializeField] 
    private PlayerController _playerController;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField] 
    private GameObject _upgradePanel;

    [SerializeField] 
    private Button _speedButton;

    [SerializeField]
    private Button _scoreButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _upgradePanel.SetActive(false);

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
        _playerController.MultiplySpeed(2f);
        StartCoroutine(ResetSpeedAfterDelay(10f, 0.5f));
        ResumeGame();
    }
    private System.Collections.IEnumerator ResetSpeedAfterDelay(float delay, float multiplier)
    {
        yield return new WaitForSeconds(delay); // zaman dursa bile iþler
      _playerController.MultiplySpeed(multiplier);
    }
    private void ChooseScore()
    {
        _gameManager.SetScoreMultiplier(2f);
        StartCoroutine(ResetScoreMultiplierAfterDelay(10f));

        ResumeGame();
    }
    private System.Collections.IEnumerator ResetScoreMultiplierAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _gameManager.SetScoreMultiplier(1f);
    }

    private void ResumeGame()
    {
        _upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
