using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public static CubeManager Instance { get; private set; }

    [SerializeField] private Transform stackFollowTarget;
    [SerializeField] private float yStep = 0.5f;
    [SerializeField] private float zOffset = 0.6f;
   

    private CubeController _lastStackedCube;
    private List<CubeController> _collectedCubes = new List<CubeController>();

    public int StackCount => _collectedCubes.Count;

    private void Awake()
    {
        Instance = this;
    }
 
    
    public void CollectCube(GameObject collectibleObj, CubeController triggeringCube)
    {
        var hitCube = collectibleObj.GetComponent<CubeController>();
        if (hitCube == null || _collectedCubes.Contains(hitCube)) return;

        hitCube.gameObject.tag = "CollectorCube";
        _collectedCubes.Add(hitCube);
        hitCube.below = _lastStackedCube;
        _lastStackedCube = hitCube;

        GameObject follow = hitCube.below != null ? hitCube.below.gameObject : stackFollowTarget.gameObject;
        Vector3 offset = hitCube.below != null ? new Vector3(0, 0f, zOffset) : new Vector3(0, yStep, zOffset);
        hitCube.SetTargetStacked(follow, offset);

        AnimateWaveDownToPlayer(triggeringCube);
        PlayerController.OnCollectibleHitEvent?.Invoke();
    }

    public void DropLastCube()
    {
        if (_lastStackedCube == null)
        {
            PlayerController.Instance.TriggerFall();
            return;
        }

        var toDrop = _lastStackedCube;
        _lastStackedCube = toDrop.below;
        toDrop.below = null;
        _collectedCubes.Remove(toDrop);
        ExplodeCube(toDrop);
    }

    public void DropFromIndex(int startIndex)
    {
        for (int i = _collectedCubes.Count - 1; i >= startIndex; i--)
        {
            var cube = _collectedCubes[i];
            if (cube == _lastStackedCube)
                _lastStackedCube = cube.below;

            _collectedCubes.RemoveAt(i);
            ExplodeCube(cube);
        }

        if (_collectedCubes.Count == 0)
        {
            PlayerController.OnObstacleHitEvent?.Invoke();
            PlayerController.Instance.TriggerFall();
        }
    }

    public void OnCubeHitObstacle(CubeController hitCube)
    {
        int idx = _collectedCubes.IndexOf(hitCube);
        if (idx < 0) return;
        DropFromIndex(idx);
    }

    private void ExplodeCube(CubeController cube)
    {
        cube.below = null;
        cube.transform.SetParent(null);
        cube.SetTargetStacked(null, Vector3.zero);
        cube.tag = "Collectible";

        var collider = cube.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
            collider.isTrigger = true;
        }

        Vector3 targetPos = new Vector3(
            Random.Range(-2.7f, 1.7f),
            0.5f,
            cube.transform.position.z + Random.Range(1.5f, 3f)
        );

        cube.transform.DOJump(targetPos, 0.8f, 1, 0.4f).SetEase(Ease.OutQuad);
    }

    private void AnimateWaveDownToPlayer(CubeController fromCube)
    {
        float maxScale = 1.0f;
        float originalScale = 0.5f;
        float duration = 0.2f;
        float delayStep = 0.05f;

        int fromIndex = _collectedCubes.IndexOf(fromCube);
        if (fromIndex < 0) return;

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
}
