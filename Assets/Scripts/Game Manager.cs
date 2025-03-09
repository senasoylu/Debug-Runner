using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void OnScoreUpdatedDelegate(int score);
    public static OnScoreUpdatedDelegate OnScoreUpdatedEvent;

    public delegate void OnGameStartedDelegate();
    public static OnGameStartedDelegate OnGameStartedEvent;

    public delegate void OnGameOverDelegate(int score);
    public static OnGameOverDelegate OnGameOverEvent;

    private int _score = 0;
    private int _addScore = 10;

    private void OnEnable()
    {
        PlayerController.OnObstacleHitEvent += OnObstacleHit;
        PlayerController.OnCollectibleHitEvent += OnCollectibleHit;

        UIManager.OnRestartButtonClickedEvent += OnRestartButtonClicked;
    }

    private void Start()
    {
        StartScreen();
    }

    private void StartScreen()
    {
        OnGameStartedEvent?.Invoke();
    }

    private void OnObstacleHit()
    {
        OnGameOverEvent?.Invoke(_score);
    }

    private void OnRestartButtonClicked()
    {
        RestartGame();
    }

    private void OnCollectibleHit()
    {
       AddScore(_addScore);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int points)
    {
        _score += points;

        OnScoreUpdatedEvent?.Invoke(_score);
    }

    private void OnDisable()
    {
        PlayerController.OnObstacleHitEvent -= OnObstacleHit;
        PlayerController.OnCollectibleHitEvent -= OnCollectibleHit;

        UIManager.OnRestartButtonClickedEvent -= OnRestartButtonClicked;
    }
}
