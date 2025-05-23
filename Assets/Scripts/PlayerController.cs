﻿using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public static System.Action OnObstacleHitEvent;
    public static System.Action OnCollectibleHitEvent;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private PlayerSettings _playerSettings;

    private PlayerSettings _originalPlayerSettings;   // Asset'in kendisi
    private PlayerSettings _runtimePlayerSettings;    // Kopyası, oyun içinde değişecek


    [SerializeField]
    private PlatformSettings _platformSettings;

    private bool _isGameStarted;
    private bool _isJumping;

    private float _jumpTimer;
    private float _groundY;

    private void Awake()
    {
        Instance = this;

        _originalPlayerSettings = _playerSettings;
        _runtimePlayerSettings = Instantiate(_playerSettings);
        _playerSettings = _runtimePlayerSettings;

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

    public PlayerSettings GetPlayerSettings()
    {
        return _playerSettings;
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

    private void OnPlayerPressedLeftButton()
    {
        if (_playerSettings.currentLaneIndex > 0)
            _playerSettings.currentLaneIndex--;
    }

    private void OnPlayerPressedRightButton()
    {
        if (_playerSettings.currentLaneIndex < _platformSettings.laneCount - 1)
            _playerSettings.currentLaneIndex++;
    }

    private void OnGameOver(int score)
    {
        _isGameStarted = false;
        _playerSettings.originalForwardSpeed = _playerSettings.playerForwardSpeed;
    }

 
    private void OnGameStarted() => _isGameStarted = true;

    private void OnJump()
    {
        if (!_isJumping)
        {
            StartJump();
        }
    }
    public void ResetSpeed()
    {
        _playerSettings.playerForwardSpeed = _playerSettings.originalForwardSpeed;
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
        float t = Mathf.Clamp01(_jumpTimer / _playerSettings.jumpDuration);
        float yOffset = _playerSettings.jumpHeight * t * (1 - t);
        float newY = _groundY + yOffset;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (_jumpTimer >= _playerSettings.jumpDuration)
        {
            _isJumping = false;
            transform.position = new Vector3(transform.position.x, _groundY, transform.position.z);
        }
    }

    private void MoveSideways()
    {
        float targetX = _platformSettings.firstLanePositionX + _playerSettings.currentLaneIndex * _platformSettings.distanceBetweenLanes;

        Vector3 desired = new Vector3(targetX, transform.position.y, transform.position.z);
        Vector3 dir = (desired - transform.position).normalized;

        float speed = _playerSettings.playerSidewaySpeed * Time.deltaTime;
        float dist = Vector3.Distance(transform.position, desired);
        transform.position += dir * Mathf.Min(speed, dist);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * _playerSettings.playerForwardSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ObstacleSettings.OBSTACLE_TAG_STRING))
        {
            OnObstacleHitEvent?.Invoke();
            CubeManager.Instance?.DropLastCube(); 
        }

        else if (other.CompareTag(CollectibleSettings.COLLECTIBLE_TAG_STRING))
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
        if (hitCube == null)
        {
            return;
        }
        CubeManager.Instance?.OnCubeHitObstacle(hitCube);
    }

    public void TriggerFall()
    {
        _animator.SetBool("isFalling", true);
        _isGameStarted = false;
    }
}
