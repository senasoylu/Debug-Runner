using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void OnScoreUpdatedDelegate(int score);
    public static event OnScoreUpdatedDelegate OnScoreUpdatedEvent;

    public delegate void OnGameStartedDelegate();
    public static event OnGameStartedDelegate OnGameStartedEvent;

    public delegate void OnGameOverDelegate(int score);
    public static event OnGameOverDelegate OnGameOverEvent;

    private int _score = 0;
    private int _addScore = 10;
    private float _scoreMultiplier = 1f;
    
    private void OnEnable()
    {
        PlayerController.OnCollectibleHitEvent += OnCollectibleHit;
        UIManager.OnRestartButtonClickedEvent += OnRestartButtonClicked;
        PlayerController.OnTriggerFallEvent += OnTriggerFall;
    }

    private void Start()
    {
        OnScoreUpdatedEvent?.Invoke(_score);
        OnGameStartedEvent?.Invoke();
    }

    public void SetScoreMultiplier(float multiplier)
    {
        _scoreMultiplier = multiplier;
    }

    private void OnCollectibleHit()
    {
        AddScore(+_addScore);
    }
    public void OnTriggerFall()
    {
        OnGameOverEvent?.Invoke(_score);
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
        ResetMultiplier(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        PlayerController.OnCollectibleHitEvent -= OnCollectibleHit;
        UIManager.OnRestartButtonClickedEvent -= OnRestartButtonClicked;
        PlayerController.OnTriggerFallEvent -= OnTriggerFall;
    }
}
