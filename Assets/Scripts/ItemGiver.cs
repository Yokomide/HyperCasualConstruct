using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] private Spender3D _resourceSpender;
    [SerializeField] private GameObject _objectToGive;
    [SerializeField] private Transform _spawnPosition;
    private TweenAnimationContoller _tweenAnimationContoller;

    private IEventActivator _eventActivator;
    private ResourceContainer3D _container;
    private bool _appearAnimationActive = true;

    [Header("====Feelings====")]
    public HapticClip haptic;
    private void OnEnable()
    {
        _eventActivator.OnEventActivate += GiveItem;
    }
    private void OnDisable()
    {
        _eventActivator.OnEventActivate -= GiveItem;
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

    private void GiveItem()
    {
        _container = _resourceSpender.Containter3D;
        var item = Instantiate(_objectToGive, _spawnPosition.position, Quaternion.identity);
        if (haptic != null)
        {
            HapticController.fallbackPreset = HapticPatterns.PresetType.Selection;
            HapticController.Play(haptic);
        }

        item.transform.localScale = new Vector3(0, 0, 0);

        Sequence _resourceSequence = DOTween.Sequence();
        _resourceSequence.Append(item.transform.DOMove(new Vector3(item.gameObject.transform.position.x, item.gameObject.transform.position.y + 3, item.gameObject.transform.position.z), 0.3f))
            .Join(item.transform.DOScale(1, 0.4f)).OnComplete(() =>
            {
                Debug.Log(_container);
                _container.Add(item.GetComponent<Resource3D>());
            });
    }

    private void AppearAnimation(GameObject target)
    {
        if (_appearAnimationActive)
        {
            target.SetActive(true);
            _tweenAnimationContoller.StartAnimation(target, 0);
            _appearAnimationActive = false;
        }
    }
}
