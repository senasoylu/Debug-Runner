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

    private bool _isJumping = false;  // Oyuncu şu anda zıplıyor mu?
    private float _jumpTimer = 0f;    // Zıplama süresi boyunca geçen zaman
    private float _groundY;           // Zıplama başlamadan önce oyuncunun yer seviyesindeki y konumu

    // Buraya ekliyoruz: en son yığılan küpü tutacak head pointer
    private CubeController _lastStackedCube;
    [SerializeField] private Transform cubeStackParent;

    const float yStep = 0.5f;
    const float zOffset = 0.5f;

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
        if (!_isGameStarted)
            return;

        MoveForward();
        MoveSideways();

        if (_isJumping)
            UpdateJump();
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
        // t, zıplamanın ilerleme oranı: 0 ile 1 arasında
        float t = Mathf.Clamp01(_jumpTimer / _gameSettings.jumpDuration);
        // Parabolik zıplama
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
        Vector3 desiredPosition = new Vector3(
            _gameSettings.firstLanePositionX + _gameSettings.currentLaneIndex * _gameSettings.distanceBetweenLanes,
            transform.position.y,
            transform.position.z
        );
        Vector3 direction = (desiredPosition - transform.position).normalized;
        float frameSpeed = _gameSettings.playerSidewaySpeed * Time.deltaTime;
        float dist = Vector3.Distance(transform.position, desiredPosition);
        transform.position += direction * Mathf.Min(frameSpeed, dist);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _gameSettings.playerForwardSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Engel ile çarpıldığında önce score –10, sonra küp düşürme işlemi:
            OnObstacleHitEvent?.Invoke();
            DropLastCube();
            PoolManager.Instance.ReturnToPool("Obstacle", other.gameObject);
        }
        else if (other.CompareTag("Collectible"))
        {
           
            OnCollectibleHitEvent?.Invoke();

            // 1) CubeController’ı al
            var hitCube = other.GetComponent<CubeController>();
            if (hitCube == null) return;

            // 2) Stack container’a taşı
           // hitCube.transform.SetParent(cubeStackParent);

            // 3) Pointer‑chain’i güncelle
            hitCube.below = _lastStackedCube;
            _lastStackedCube = hitCube;

            // 4) Takip edilecek hedef ve offset’i ayarla
            GameObject follow = (hitCube.below != null)
                ? hitCube.below.gameObject
                : gameObject;
            Vector3 offset = (hitCube.below != null)
                ? new Vector3(0, 0f, zOffset)
                : new Vector3(0, yStep, zOffset);

            hitCube.SetTargetStacked(follow, offset);

            // (Opsiyonel) Havuzlama devreyse:
            // PoolManager.Instance.ReturnToPool("Collectible", other.gameObject);
        }
    }


    /// <summary>
    /// Head pointer’ta tuttuğumuz en son küpü çıkarır.
    /// Eğer küp yoksa TriggerFall() ile game over olur.
    /// </summary>
    private void DropLastCube()
    {
        if (_lastStackedCube == null)
        {
            TriggerFall();
            return;
        }
        var toDrop = _lastStackedCube;
        _lastStackedCube = toDrop.below;
        toDrop.below = null;
      //  toDrop.transform.SetParent(null);
        toDrop.gameObject.SetActive(false);
    }


    private void TriggerFall()
    {
        animator.SetBool("isFalling", true);
        _isGameStarted = false;
    }
}
