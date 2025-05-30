using System;
using DG.Tweening;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static System.Action OnCollectibleHitEvent;
    public static System.Action OnTriggerFallEvent;

    public static event Action<Vector3> OnPositionChangeEvent;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private PlayerSettings _playerSettings;

    [SerializeField]
    private PlatformSettings _platformSettings;

    private bool _isGameStarted;
    private bool _isJumping;

    private float _jumpTimer;
    private float _groundY;

    private float _currentSpeed;

    [SerializeField]
    private PlayerNavigationData _playerNavigationData;


    private void Awake()
    {
        _currentSpeed=_playerSettings.originalForwardSpeed;

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
        _playerNavigationData.Subscribe();
        OnPositionChangeEvent?.Invoke(transform.position);

    }

    private void OnDisable()
    {
        InputController.SwipeLeftEvent -= OnPlayerPressedLeftButton;
        InputController.SwipeRightEvent -= OnPlayerPressedRightButton;
        InputController.JumpEvent -= OnJump;

        GameManager.OnGameOverEvent -= OnGameOver;
        GameManager.OnGameStartedEvent -= OnGameStarted;
        _playerNavigationData.Unsubscribe();
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
    }

    private void OnGameStarted() => _isGameStarted = true;

    private void OnJump()
    {
        if (!_isJumping)
        {
            StartJump();
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public void ResetSpeed()
    {
        _currentSpeed = _playerSettings.playerForwardSpeed;
    }

    public void MultiplySpeed(float multiplier)
    {
        _currentSpeed *= multiplier;
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
        transform.Translate(Vector3.forward * _currentSpeed * Time.deltaTime);
        OnPositionChangeEvent?.Invoke(transform.position);
    }

    public void TriggerFall()
    {
        _animator.SetBool("isFalling", true);
        _isGameStarted = false; 
        OnTriggerFallEvent?.Invoke();   
    }
}
