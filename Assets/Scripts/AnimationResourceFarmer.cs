using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class AnimationResourceFarmer : MonoBehaviour
{
    [Header("====Resource Settings====")]
    [SerializeField] private ResourceType _resourceType;
    [Space(10)]

    [Header("====Farm Settings====")]
    [SerializeField] private float _farmDelay;
    [SerializeField] private float _farmSpeed;
    [SerializeField] private float _farmCancelSpeed;
    [SerializeField] private int _amountPerTick;
    [Space(10)]

    [Header("====Limit====")]
    [SerializeField] private bool LimitCapacity;
    [SerializeField, ConditionalHide("LimitCapacity", true)] private bool _startAmountEqualCapacity;
    [SerializeField, ConditionalHide("LimitCapacity", true)] private int _capacity;
    [SerializeField, ConditionalHide("LimitCapacity", true)] private int _resourceAmount;
    [Space(10)]

    [Header("====Animation====")]
    [SerializeField] private CharacterAnimationType _characterAnimationType;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _lookAtTarget;
    [Space(10)]

    [Header("====Visual====")]

    [SerializeField] private bool UseResourceModel;
    [SerializeField, ConditionalHide("UseResourceModel", true)] private GameObject _resourceModel;
    [SerializeField] private GameObject _vfx;
    [SerializeField] private bool ResourceToUIAnim;
    [SerializeField, ConditionalHide("ResourceToUIAnim", true)] private GameObject _resourceVisual;
    [SerializeField, ConditionalHide("ResourceToUIAnim", true)] private GameObject _uiResourceTransform;
    [SerializeField, ConditionalHide("ResourceToUIAnim", true)] private GameObject _uiNewTransform;

    [SerializeField] private Camera cam;

    private ICollector _collector;
    private GameObject _interactor;
    private AnimationEventsHolder _animEventsHolder;

    private bool _eventSubscribed;
    private Sequence _resourceSequence;

    private bool _isFarmingActive;

    private void Start()
    {
        if (_startAmountEqualCapacity)
        {
            _resourceAmount = _capacity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        if (!other.TryGetComponent(out AnimationEventsHolder animEvent))
            return;
        if (LimitCapacity && _resourceAmount < _amountPerTick)
            return;
        _interactor = other.gameObject;
        _collector = collector;
        _animEventsHolder = animEvent;
        _isFarmingActive = true;
        EventSubscribe();
        StartFarming();

    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        StopFarming();
    }
    private void StartFarming()
    {
        _vfx.SetActive(false);
        if (!_isFarmingActive)
        {
            StopFarming();
            return;
        }

        _collector.StartAnimation(_characterAnimationType);

            
    }
    private void StopFarming()
    {
        _vfx.SetActive(false);

        if (_eventSubscribed)
            EventUnsubscribe();
        _collector.EndAnimation();
        _interactor = null;
        _collector = null;
        _animEventsHolder = null;
        _isFarmingActive = false;

    }

    private void SetCollectorResource()
    {
        _vfx.SetActive(false);
        _vfx.SetActive(true);
        if (LimitCapacity)
        {
            if (_resourceAmount >= _amountPerTick)
            {
                _resourceAmount -= _amountPerTick;
                _collector.AddResource(_amountPerTick, _resourceType);

                if (UseResourceModel && _interactor.TryGetComponent(out ResourceContainer3D resourceContainer3D))
                    SpawnResourceModel(resourceContainer3D);

                if (ResourceToUIAnim)
                    SpawnResourceVisual();
            }
            else
            {
                StopFarming();
            }
        }
        else
        {
            _collector.AddResource(_amountPerTick, _resourceType);

            if (UseResourceModel && _interactor.TryGetComponent(out ResourceContainer3D resourceContainer3D))
                SpawnResourceModel(resourceContainer3D);

            if (ResourceToUIAnim)
                SpawnResourceVisual();
        }
    }

    private void SpawnResourceVisual()
    {
        var resourceVisual = Instantiate(_resourceVisual, gameObject.transform);

        _resourceSequence = DOTween.Sequence();
        _resourceSequence.Append(resourceVisual.transform.DOMove(new Vector3(resourceVisual.gameObject.transform.position.x, resourceVisual.gameObject.transform.position.y + 2, resourceVisual.gameObject.transform.position.z), 0.4f))
            .Join(resourceVisual.gameObject.transform.DOScale(1f, 0.8f))
            .Append(resourceVisual.gameObject.transform.DOScale(0f, 0.3f))
            .OnComplete(() => Destroy(resourceVisual));
    }

    private void SpawnResourceModel(ResourceContainer3D resourceContainer3D)
    {
        var resource3D = Instantiate(_resourceModel, _target.position, Quaternion.identity);
        resource3D.transform.localScale = new Vector3(0, 0, 0);
        _resourceSequence = DOTween.Sequence();
        _resourceSequence.Append(resource3D.transform.DOMove(new Vector3(resource3D.gameObject.transform.position.x, resource3D.gameObject.transform.position.y + 3, resource3D.gameObject.transform.position.z), 0.3f))
            .Join(resource3D.transform.DOScale(1, 0.4f)).OnComplete(() => resourceContainer3D.Add(resource3D));

    }
    private void SetLookAt()
    {
        _interactor.transform.DOLookAt(new Vector3(_target.position.x, _interactor.transform.position.y, _target.position.z), 0.25f);
    }
    private void EventSubscribe()
    {
        _animEventsHolder.OnAnimationAction += SetCollectorResource;
        _animEventsHolder.OnAnimationEnd += StartFarming;

        if (_lookAtTarget)
            _animEventsHolder.OnAnimationLook += SetLookAt;

        _eventSubscribed = true;
    }
    private void EventUnsubscribe()
    {
        _animEventsHolder.OnAnimationAction -= SetCollectorResource;
        _animEventsHolder.OnAnimationEnd -= StartFarming;

        if (_lookAtTarget)
            _animEventsHolder.OnAnimationLook -= SetLookAt;

        _eventSubscribed = false;
    }
}