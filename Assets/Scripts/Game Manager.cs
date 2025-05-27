using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public delegate void OnScoreUpdatedDelegate(int score);
    public static OnScoreUpdatedDelegate OnScoreUpdatedEvent;

    public delegate void OnGameStartedDelegate();
    public static OnGameStartedDelegate OnGameStartedEvent;

    public delegate void OnGameOverDelegate(int score);
    public static OnGameOverDelegate OnGameOverEvent;

    private int _score = 0;
    private int _addScore = 10;
    private float _scoreMultiplier = 1f;
    

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

    private void Awake()
    {
        Instance = this;
    }
    public void SetScoreMultiplier(float multiplier)
    {
        _scoreMultiplier = multiplier;
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
        _score += Mathf.RoundToInt(delta * _scoreMultiplier);
        OnScoreUpdatedEvent?.Invoke(_score);
    }

    private void OnRestartButtonClicked()
    {
        RestartGame();
    }
    public void ResetMultiplier()
    {
        _scoreMultiplier = 1f;
    }

    public void RestartGame()
    {
        ResetMultiplier(); // Skor çarpanýný sýfýrla
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        PlayerController.OnObstacleHitEvent -= OnObstacleHit;
        PlayerController.OnCollectibleHitEvent -= OnCollectibleHit;
        UIManager.OnRestartButtonClickedEvent -= OnRestartButtonClicked;
    }
}
