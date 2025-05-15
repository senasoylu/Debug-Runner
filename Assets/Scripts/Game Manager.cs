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
        OnScoreUpdatedEvent?.Invoke(_score);
        OnGameStartedEvent?.Invoke();
    }

    private void OnCollectibleHit()
    {
        AddScore(+_addScore);
    }

    private void OnObstacleHit()
    {
        if (_score >= _addScore)
        {
            AddScore(-_addScore);
        }
        else
        {
            OnGameOverEvent?.Invoke(_score);
        }
    }

    public void AddScore(int delta)
    {
        _score += delta;
        OnScoreUpdatedEvent?.Invoke(_score);
    }

    private void OnRestartButtonClicked()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        PlayerController.OnObstacleHitEvent -= OnObstacleHit;
        PlayerController.OnCollectibleHitEvent -= OnCollectibleHit;
        UIManager.OnRestartButtonClickedEvent -= OnRestartButtonClicked;
    }
}
