using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnObstacleHitDelegate();
    public static OnObstacleHitDelegate OnObstacleHitEvent;

    public delegate void OnCollectibleHitDelegate();
    public static OnCollectibleHitDelegate OnCollectibleHitEvent;

    private GameSettings _gameSettings;

    public Animator animator;

    private bool _isGameStarted;
   
    private void OnEnable()
    {
        InputController.OnPlayerPressedLeftButtonEvent += OnPlayerPressedLeftButton;
        InputController.OnPlayerPressedRightButtonEvent += OnPlayerPressedRightButton;

        GameManager.OnGameOverEvent += OnGameOver;
        GameManager.OnGameStartedEvent += OnGameStarted;
    }
    private void OnPlayerPressedLeftButton()
    {
        if (_gameSettings.currentLaneIndex > 0)
        {
            _gameSettings.currentLaneIndex--;
        }
    }
    private void OnPlayerPressedRightButton()
    {
        if (_gameSettings.currentLaneIndex < _gameSettings.laneCount)
        {
            _gameSettings.currentLaneIndex++;
        }
    }

    private void OnGameOver(int score)
    {
        _isGameStarted = false;
    }

    private void OnGameStarted()
    {
        _isGameStarted = true;
    }

    private void OnDisable()
    {
        InputController.OnPlayerPressedLeftButtonEvent -= OnPlayerPressedLeftButton;
        InputController.OnPlayerPressedRightButtonEvent -= OnPlayerPressedRightButton;

        GameManager.OnGameOverEvent -= OnGameOver;
        GameManager.OnGameStartedEvent -= OnGameStarted;
    }

    private void Start()
    {
        _gameSettings = FindObjectOfType<GameSettings>(); 
         animator = GetComponent<Animator>();
    }
  
    private void Update()
    {
        if (!_isGameStarted)
        {
            return;
        }

        MoveForward();
        MoveSideways();
    }

    private void MoveSideways()
    {
        Vector3 desiredPosition;  // hedef pozisyon
        desiredPosition = new Vector3(_gameSettings.firstLanePositionX + _gameSettings.currentLaneIndex * _gameSettings.distanceBetweenLanes, transform.position.y, transform.position.z);
        Vector3 currentPosition = transform.position;
        Vector3 direction = desiredPosition - currentPosition;
        direction = Vector3.Normalize(direction);

        float frameSpeed = _gameSettings.playerSidewaySpeed * Time.deltaTime;
        float distance = Vector3.Distance(currentPosition, desiredPosition);

        if (distance < frameSpeed)
        {
            frameSpeed = distance;
        }
        transform.position = currentPosition + direction * frameSpeed;
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _gameSettings.playerForwardSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            OnObstacleHitEvent?.Invoke();
            TriggerFall();
        }
        else if (other.CompareTag("Collectible"))
        {
            OnCollectibleHitEvent?.Invoke();
            other.gameObject.SetActive(false);
        }
    }
    private void TriggerFall()
    {
        animator.SetBool("isFalling", true);
        _isGameStarted = false;
    }
}

