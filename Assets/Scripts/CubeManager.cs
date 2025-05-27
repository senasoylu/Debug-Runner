using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public static CubeManager Instance { get; private set; }


    [SerializeField]
    private CollectibleSettings _collectibleSettings;

    private CubeController _lastStackedCube;

    private List<CubeController> _collectedCubes = new List<CubeController>();

    public int StackCount => _collectedCubes.Count;

    private int _explodeIndexCounter = 0;

    [SerializeField]
    private PlayerController _player;

    private void Awake()
    {
        Instance = this;
        ListenEvent();
    }
    private void ListenEvent()
    {
        CubeController.OnObstacleHitEvent += OnCubeHitObstacle;
    }
  

    public void CollectCube(GameObject collectibleObj, CubeController triggeringCube)
    {
        CubeController hitCube = collectibleObj.GetComponent<CubeController>();
        if (hitCube == null || _collectedCubes.Contains(hitCube))
        {
            return;
        }
           
        hitCube.gameObject.tag = "CollectorCube";
        _collectedCubes.Add(hitCube);

        hitCube.below = _lastStackedCube;
        _lastStackedCube = hitCube;

        GameObject follow = hitCube.below != null 
            ? hitCube.below.gameObject 
            : _player.gameObject;

        Vector3 offset = hitCube.below != null
            ? new Vector3(0, 0f, _collectibleSettings.Z_OFFSET) 
            : new Vector3(0,_collectibleSettings.Y_STEP, _collectibleSettings.Z_OFFSET);

        hitCube.SetTargetStacked(follow, offset);

        AnimateWaveDownToPlayer(triggeringCube);

        PlayerController.OnCollectibleHitEvent?.Invoke();
    }

    public void DropLastCube()
    {
        if (_lastStackedCube == null)
        {
            _player.TriggerFall();
            return;
        }

        CubeController toDrop = _lastStackedCube;
        _lastStackedCube = toDrop.below;
        toDrop.below = null;
        _collectedCubes.Remove(toDrop);
        ExplodeCube(toDrop);
    }

    public void DropFromIndex(int startIndex)
    {
        for (int i = _collectedCubes.Count - 1; i >= startIndex; i--)
        {
            CubeController cube = _collectedCubes[i];

            if (cube == _lastStackedCube)
                _lastStackedCube = cube.below;

            _collectedCubes.RemoveAt(i);
            ExplodeCube(cube);
        }

        if (_collectedCubes.Count == 0)
        {
            GameManager.OnGameOverEvent?.Invoke(0);
           _player.TriggerFall();
        }
    }

    public void OnCubeHitObstacle(CubeController hitCube)
    {
        int hitCubeIndex = _collectedCubes.IndexOf(hitCube);

        if (hitCubeIndex < 0)
        {
            return;
        }
        DropFromIndex(hitCubeIndex);
    }
  
    private void ExplodeCube(CubeController cube)
    {
        cube.below = null;
        cube.transform.SetParent(null);
        cube.SetTargetStacked(null, Vector3.zero);
        cube.tag = CollectibleSettings.COLLECTIBLE_TAG_STRING;

        Collider collider = cube.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
            collider.isTrigger = true;
        }

        const float xRandomDistanceLeft = -2.7f;
        const float xRandomDistanceRight = 1.7f;
        const float zMinDistance = 4.0f;
        const float zMaxDistance = 16.0f;

        const float jumpDuration = 0.4f;
        const float jumpPower = 0.6f;
        const int numberOfJump = 1;

        float spreadX = Random.Range(xRandomDistanceLeft, xRandomDistanceRight);
        float spreadZ = Random.Range(zMinDistance, zMaxDistance);

        float baseZ = _lastStackedCube != null
            ? _lastStackedCube.transform.position.z
            : cube.transform.position.z;

        Vector3 targetPos = new Vector3(spreadX, 0.5f, baseZ + spreadZ);

        cube.transform.DOJump(targetPos, jumpPower, numberOfJump, jumpDuration)
            .SetEase(Ease.OutQuad);

        _explodeIndexCounter++;
    }

    private void AnimateWaveDownToPlayer(CubeController fromCube)
    {
        const float maxScale = 1.0f;
        const float originalScale = 0.5f;
        const float duration = 0.2f;
        const float delayStep = 0.05f;
       
        int fromIndex = _collectedCubes.IndexOf(fromCube);

        if (fromIndex < 0)
        {
            return;
        }

        int step = 0;
        for (int i = fromIndex; i >= 0; i--)
        {
            var t = _collectedCubes[i].transform;
            float delay = step * delayStep;
            t.DOKill();
            DOTween.Sequence()
                .PrependInterval(delay)
                .Append(t.DOScale(maxScale, duration))
                .Append(t.DOScale(originalScale, duration));
            step++;
        }
    }
    private void Unsubscribe()
    {
        CubeController.OnObstacleHitEvent -= OnCubeHitObstacle;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
