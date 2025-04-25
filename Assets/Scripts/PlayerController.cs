// PlayerController.cs
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public delegate void OnObstacleHitDelegate();
    public static OnObstacleHitDelegate OnObstacleHitEvent;

    public delegate void OnCollectibleHitDelegate();
    public static OnCollectibleHitDelegate OnCollectibleHitEvent;

    [SerializeField] private Transform cubeStackParent;

    private GameSettings _gameSettings;
    public Animator animator;

    private bool _isGameStarted;
    private bool _isJumping;
    private float _jumpTimer;
    private float _groundY;

    // Head pointer: en son yığılan küp
    private CubeController _lastStackedCube;

    // Stack’teki tüm küpleri sıralı tutan liste
    [SerializeField] private List<CubeController> _collectedCubes = new List<CubeController>();

    const float yStep = 0.5f;
    const float zOffset = 0.6f;

    private void Awake()
    {
        Instance = this;
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
        float targetX = _gameSettings.firstLanePositionX
                      + _gameSettings.currentLaneIndex * _gameSettings.distanceBetweenLanes;
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
            // Player obstacle çarpışmasında sadece son küp koparsın:
            DropLastCube();
            PoolManager.Instance.ReturnToPool("Obstacle", other.gameObject);
        }
        else if (other.CompareTag("Collectible"))
        {
            CollectCube(other.gameObject);
        }
    }

    /// <summary>
    /// Yeni collectible toplandığında listeye ekler ve pointer-chain günceller.
    /// </summary>
    public void CollectCube(GameObject collectibleObj)
    {
        OnCollectibleHitEvent?.Invoke();
        var hitCube = collectibleObj.GetComponent<CubeController>();
        if (hitCube == null) return;

        // Tag ayarlaması
        hitCube.gameObject.tag = "CollectorCube";

        if (!_collectedCubes.Contains(hitCube))
            _collectedCubes.Add(hitCube);

        hitCube.below = _lastStackedCube;
        _lastStackedCube = hitCube;

        GameObject follow = hitCube.below != null ? hitCube.below.gameObject : gameObject;
        Vector3 offset = hitCube.below != null
            ? new Vector3(0, 0f, zOffset)
            : new Vector3(0, yStep, zOffset);
        hitCube.SetTargetStacked(follow, offset);
    }

    /// <summary>
    /// Collected listesinden startIndex ve sonrası küpleri koparır.
    /// </summary>
    public void DropFromIndex(int startIndex)
    {
        if (startIndex < 0 || startIndex >= _collectedCubes.Count) return;
        for (int i = _collectedCubes.Count - 1; i >= startIndex; i--)
        {
            var cube = _collectedCubes[i];
            if (cube == _lastStackedCube)
                _lastStackedCube = cube.below;
            cube.below = null;
            _collectedCubes.RemoveAt(i);
            cube.transform.SetParent(null);
            cube.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Bir stacked küp obstacle çarptığında çağrılır.
    /// </summary>
    public void OnObstacleHitByCube(CubeController hitCube)
    {
        int idx = _collectedCubes.IndexOf(hitCube);
        if (idx < 0) return;
        DropFromIndex(idx);

        // Eğer yığın tamamen boşaldıysa GAME OVER
        if (_collectedCubes.Count == 0)
        {
            OnObstacleHitEvent?.Invoke();
            TriggerFall();
        }
    }

    /// <summary>
    /// Oyuncunun kendisi obstacle çarptığında son küpü koparır.
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
        if (_collectedCubes.Contains(toDrop))
            _collectedCubes.Remove(toDrop);
        toDrop.transform.SetParent(null);
    }

    private void TriggerFall()
    {
        animator.SetBool("isFalling", true);
        _isGameStarted = false;
    }
}
