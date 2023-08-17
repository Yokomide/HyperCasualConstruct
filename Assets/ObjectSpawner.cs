using Lofelt.NiceVibrations;
using System;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Action<GameObject> OnObjectsSpawned;
    [SerializeField] private Spender3D _resourceSpender;
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private GameObject _vfx;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Quaternion _spawnRotation;

    private TweenAnimationContoller _tweenAnimationContoller;

    private IEventActivator _eventActivator;
    private bool _appearAnimationActive = true;

    [Header("====Feelings====")]
    public HapticClip haptic;
    private void OnEnable()
    {
        _eventActivator.OnEventActivate += SpawnObject;
        _tweenAnimationContoller.OnAnimationEnd += ResetAnimationStatus;
    }
    private void OnDisable()
    {
        _eventActivator.OnEventActivate -= SpawnObject;
        _tweenAnimationContoller.OnAnimationEnd -= ResetAnimationStatus;

    }
    private void Awake()
    {
        _tweenAnimationContoller = GetComponent<TweenAnimationContoller>();

        if (_resourceSpender.TryGetComponent(out IEventActivator eventActivator))
        {
            _eventActivator = eventActivator;
        }
        else
        {
            Debug.LogError("GameObject have no Script which realize EventActivator Interface");
        }

    }
    public void SetObjectToSpawn(GameObject gameObject)
    {
        _objectToSpawn = gameObject;
    }

    private void SpawnObject()
    {
        _vfx.SetActive(false);
        var spawnedObject = Instantiate(_objectToSpawn, _spawnPosition.position, _spawnRotation);
        spawnedObject.SetActive(false);
        if (haptic != null)
        {
            if (HapticController.hapticsEnabled)
            {
                HapticController.Play(haptic);

            }
            else
            {
                HapticController.fallbackPreset = HapticPatterns.PresetType.Selection;
            }

        }
        _vfx.SetActive(true);
        AppearAnimation(spawnedObject);
        OnObjectsSpawned?.Invoke(spawnedObject);
    }

    private void AppearAnimation(GameObject target)
    {
        if (_appearAnimationActive)
        {
            target.SetActive(true);
            _tweenAnimationContoller.StartAnimation(target, 0);
            //_appearAnimationActive = false;
        }
    }

    private void ResetAnimationStatus()
    {
        _appearAnimationActive = true;
    }
}
