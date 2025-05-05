using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public static  System.Action OnObstacleHitEvent;
    public static  System.Action OnCollectibleHitEvent;

    [SerializeField] private Transform cubeStackParent;

    private GameSettings _gameSettings;
    public Animator animator;

    private bool _isGameStarted;
    private bool _isJumping;
    private float _jumpTimer;
    private float _groundY;

    private void Awake()
    {
        Instance = this;
        DOTween.SetTweensCapacity(500, 250);
        DOTween.Init();
    }

    private void OnEnable()
    {
        InputController.SwipeLeftEvent += OnPlayerPressedLeftButton;
        InputController.SwipeRightEvent += OnPlayerPressedRightButton;
        InputController.JumpEvent += OnJump;

        GameManager.OnGameOverEvent += OnGameOver;
        GameManager.OnGameStartedEvent += OnGameStarted;
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
        if (!_isGameStarted) return;
        MoveForward();
        MoveSideways();
        if (_isJumping) UpdateJump();
    }

    private void OnPlayerPressedLeftButton()
    {
        if (_gameSettings.currentLaneIndex > 0)
            _gameSettings.currentLaneIndex--;
    }

    private void OnPlayerPressedRightButton()
    {
        if (_gameSettings.currentLaneIndex < _gameSettings.laneCount - 1)
            _gameSettings.currentLaneIndex++;
    }

    private void OnGameOver(int score) => _isGameStarted = false;
    private void OnGameStarted() => _isGameStarted = true;

    private void OnJump()
    {
        if (!_isJumping) StartJump();
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
        float t = Mathf.Clamp01(_jumpTimer / _gameSettings.jumpDuration);
        float yOffset = 4 * _gameSettings.jumpHeight * t * (1 - t);
        float newY = _groundY + yOffset;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        if (_jumpTimer >= _gameSettings.jumpDuration)
        {
            _isJumping = false;
            transform.position = new Vector3(transform.position.x, _groundY, transform.position.z);
        }
    }

    private void MoveSideways()
    {
        float targetX = _gameSettings.firstLanePositionX + _gameSettings.currentLaneIndex * _gameSettings.distanceBetweenLanes;
        Vector3 desired = new Vector3(targetX, transform.position.y, transform.position.z);
        Vector3 dir = (desired - transform.position).normalized;
        float speed = _gameSettings.playerSidewaySpeed * Time.deltaTime;
        float dist = Vector3.Distance(transform.position, desired);
        transform.position += dir * Mathf.Min(speed, dist);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _gameSettings.playerForwardSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            OnObstacleHitEvent?.Invoke();
            CubeManager.Instance?.DropLastCube();
            PoolManager.Instance?.ReturnToPool("Obstacle", other.gameObject);
        }
        else if (other.CompareTag("Collectible"))
        {
            var cube = other.GetComponent<CubeController>();
            if (cube != null)
            {
                CubeManager.Instance?.CollectCube(other.gameObject, cube);
                OnCollectibleHitEvent?.Invoke();
            }
        }
    }

    public void OnObstacleHitByCube(CubeController hitCube)
    {
        if (hitCube == null) return;
        CubeManager.Instance?.OnCubeHitObstacle(hitCube);
    }

    public void TriggerFall()
    {
        animator.SetBool("isFalling", true);
        _isGameStarted = false;
    }
}
