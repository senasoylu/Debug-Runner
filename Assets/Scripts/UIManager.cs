using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lastScoreText;
    public TextMeshProUGUI gameOverText;

    public Button restartButton;
    public Button startScreen;

    public delegate void OnRestartButtonClickedDelegate();
    public static OnRestartButtonClickedDelegate OnRestartButtonClickedEvent;

    private void OnEnable()
    {
        GameManager.OnScoreUpdatedEvent += OnScoreUpdated;
        GameManager.OnGameOverEvent += OnGameOver;
    }

    private void Start()
    {
        startScreen.onClick.AddListener(StartScreen);
        startScreen.gameObject.SetActive(true);

        restartButton.onClick.AddListener(Restart);
        restartButton.gameObject.SetActive(false);

        scoreText.gameObject.SetActive(true);

        gameOverText.gameObject.SetActive(false);

        lastScoreText.gameObject.SetActive(false);
        OnScoreUpdated(0);
    }

    public void Restart()
    {
        OnRestartButtonClickedEvent?.Invoke();
    }

    public void StartScreen()
    {
        startScreen.gameObject.SetActive(false);
    }

    private void ShowGameOver(int score)
    {
        gameOverText.text = "Game Over";
        gameOverText.gameObject.SetActive(true);

        lastScoreText.text = "Last Score: " + score;
        lastScoreText.gameObject.SetActive(true);

        scoreText.gameObject.SetActive(false);

        restartButton.gameObject.SetActive(true);
    }

    private void OnScoreUpdated(int score)
    {
        scoreText.text = "Score: " + score;
    }
  
    private void OnGameOver(int score)
    {
        ShowGameOver(score);
    }

    private void OnDisable()
    {
        GameManager.OnScoreUpdatedEvent -= OnScoreUpdated;
        GameManager.OnGameOverEvent -= OnGameOver;
    }
}
