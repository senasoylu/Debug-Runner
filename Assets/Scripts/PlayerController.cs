using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
        InputController.SwipeLeftEvent += OnPlayerPressedLeftButton;
        InputController.SwipeRightEvent += OnPlayerPressedRightButton;
        InputController.JumpEvent += OnJump;

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
        if (_gameSettings.currentLaneIndex < _gameSettings.laneCount - 1)
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
    private void OnJump()
    {
        if (!_gameSettings.isJumping)
            StartJump();
    }
    private void StartJump()
    {
        _gameSettings.isJumping = true;
        _gameSettings.jumpTimer = 0f;
        _gameSettings.groundY = transform.position.y;
    }
    private void UpdateJump()
    {
        _gameSettings.jumpTimer += Time.deltaTime;
        // t, zýplamanýn ilerleme oranýný temsil eder: 0 (baþlangýç) ile 1 (bitiþ) arasýnda
        float t = _gameSettings.jumpTimer / _gameSettings.jumpDuration;
        t = Mathf.Clamp01(t);

        // Parabolik zýplama eðrisi:
        // y_offset = 4 * jumpHeight * t * (1-t)
        // Bu formüle göre, t = 0 veya 1 olduðunda y_offset 0, t = 0.5'te y_offset maksimum (jumpHeight) deðerine ulaþýr
        float yOffset = 4 * _gameSettings.jumpHeight * t * (1 - t);
        float newY = _gameSettings.groundY + yOffset;

        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;

        // Zýplama süresi tamamlandýðýnda zýplamayý sonlandýr
        if (_gameSettings.jumpTimer >= _gameSettings.jumpDuration)
        {
            _gameSettings.isJumping = false;
            pos.y = _gameSettings.groundY;
            transform.position = pos;
        }
    }


    private void OnDisable()
    {
        InputController.SwipeLeftEvent -= OnPlayerPressedLeftButton;
        InputController.SwipeRightEvent -= OnPlayerPressedRightButton;
        InputController.JumpEvent -= OnJump;

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

        if (_gameSettings.isJumping)
        {
            UpdateJump();
        }
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

