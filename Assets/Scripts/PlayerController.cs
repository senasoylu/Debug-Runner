// PlayerController.cs
using DG.Tweening;
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

    public Transform[] scalingTransform;
    public float maxScaleValue = 1.2f;
    public float scalingTime = 1.0f;

    private CubeController _collectorCube;

    // Head pointer: en son yığılan küp
    private CubeController _lastStackedCube;

    // Stack’teki tüm küpleri sıralı tutan liste
    [SerializeField] private List<CubeController> _collectedCubes = new List<CubeController>();

    const float yStep = 0.5f;
    const float zOffset = 0.6f;

    private void Awake()
    {
        Instance = this;
       
            // En azından _tweenCount kadar Tween, _sequenceCount kadar Sequence baştan hazır olsun
            DOTween.SetTweensCapacity(
                tweenersCapacity: 500,  // örneğin 500 tane tekil Tween
                sequencesCapacity: 250   // örneğin 250 tane Sequence
            );
            DOTween.Init();  // isteğe bağlı, otomatik init yerine kontrolü elinize alırsınız
        
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
            DropLastCube();
            PoolManager.Instance.ReturnToPool("Obstacle", other.gameObject);
        }
        else if (other.CompareTag("Collectible"))
        {
            CollectCube(other.gameObject);
        }
    }

    public void CollectCube(GameObject collectibleObj)
    {
        OnCollectibleHitEvent?.Invoke();
        var hitCube = collectibleObj.GetComponent<CubeController>();
        if (hitCube == null) return;

        // 1) “Yeniden toplanan” küpü stopper olarak sakla
        _collectorCube = hitCube;
        // 2) O küpe sadece bir kez tag ata
        hitCube.gameObject.tag = "CollectorCube";

        // 3) Listeye ekle (aynı küp birden çok eklenmesin)
        if (!_collectedCubes.Contains(hitCube))
            _collectedCubes.Add(hitCube);

        // 4) Stacking bağlantılarını kur
        hitCube.below = _lastStackedCube;
        _lastStackedCube = hitCube;

        GameObject follow = hitCube.below != null ? hitCube.below.gameObject : gameObject;
        Vector3 offset = hitCube.below != null
            ? new Vector3(0, 0f, zOffset)
            : new Vector3(0, yStep, zOffset);
        hitCube.SetTargetStacked(follow, offset);

        // 5) Dalga animasyonunu, yeni toplananın index’inden başlat
        int startIndex = _collectedCubes.IndexOf(_collectorCube);
        AnimateWaveFrom(startIndex);
        
      

    }
    private void AnimateWaveFrom(int startIndex)
    {
        const float maxScale = 0.8f;
        const float originalScale = 0.5f;
        const float duration = 0.2f;
        const float delayStep = 0.05f;

        if (startIndex <= 0) return;

        for (int i = startIndex - 1; i >= 0; i--)
        {
            int step = (startIndex - 1) - i;
            float delay = step * delayStep;
            Transform t = _collectedCubes[i].transform;

            t.DOKill();
            DOTween.Sequence()
                   .PrependInterval(delay)
                   .Append(t.DOScale(maxScale, duration))
                   .Append(t.DOScale(originalScale, duration));
        }
    }


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
