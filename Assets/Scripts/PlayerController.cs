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

    private bool _isJumping = false;  // Oyuncu þu anda zýplýyor mu?
    private float _jumpTimer = 0f;    // Zýplama süresi boyunca geçen zaman
    private float _groundY;           // Zýplama baþlamadan önce oyuncunun yer seviyesindeki y konumu


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
        if (!_isJumping)
            StartJump(); 
    }
    private void StartJump()
    {
        _isJumping = true;
        _jumpTimer = 0f;
        _groundY = transform.position.y;
    }
    private void UpdateJump()
    {
        _jumpTimer += Time.deltaTime;
        // t, zýplamanýn ilerleme oranýný temsil eder: 0 (baþlangýç) ile 1 (bitiþ) arasýnda
        float t = _jumpTimer / _gameSettings.jumpDuration;
        t = Mathf.Clamp01(t);

        // Parabolik zýplama eðrisi:
        // y_offset = 4 * jumpHeight * t * (1-t)
        // Bu formüle göre, t = 0 veya 1 olduðunda y_offset 0, t = 0.5'te y_offset maksimum (jumpHeight) deðerine ulaþýr
        float yOffset = 4 * _gameSettings.jumpHeight * t * (1 - t);
        float newY = _groundY + yOffset;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Zýplama süresi tamamlandýðýnda zýplamayý sonlandýr
        if (_jumpTimer >= _gameSettings.jumpDuration)
        {
            _isJumping = false;
            transform.position = new Vector3(transform.position.x, _groundY, transform.position.z);
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

        if (_isJumping)
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
            ObstacleController.Instance.ReturnObstacle( other.gameObject);
        }

        else if (other.CompareTag("Collectible"))
        {
            OnCollectibleHitEvent?.Invoke();
            PoolManager.Instance.ReturnToPool("Collectible",other.gameObject);
            // CollectibleController üzerinden havuza geri gönder
         
           
        }
    }

    private void TriggerFall()
    {
        animator.SetBool("isFalling", true);
        _isGameStarted = false;
    }
}

