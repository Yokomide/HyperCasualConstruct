using System.Collections.ObjectModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;
using Sirenix.OdinInspector;
public class ResourceFarmer : MonoBehaviour
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
    [SerializeField] private bool CollectAnimation;
    [Space(10)]

    [Header("====Visual====")]
    [OnValueChanged("ChangeFillColor")]
    [SerializeField] private Color _fillColor;
    [SerializeField] private Image _fillImage;

    [SerializeField] private bool UseResourceModel;
    [SerializeField, ConditionalHide("UseResourceModel", true)] private GameObject _resourceModel;

    [SerializeField] private bool ResourceToUIAnim;
    [SerializeField, ConditionalHide("ResourceToUIAnim", true)] private GameObject _resourceVisual;
    [SerializeField, ConditionalHide("ResourceToUIAnim", true)] private GameObject _uiResourceTransform;
    [SerializeField, ConditionalHide("ResourceToUIAnim", true)] private GameObject _uiNewTransform;

    [SerializeField] private Camera cam;



    private Sequence _farmSequence;
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
        if (LimitCapacity && _resourceAmount < _amountPerTick)
            return;
        _isFarmingActive = true;
        StartFarming(collector, other.gameObject);

        if (CollectAnimation)
            collector.StartCollectAnimation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        StopFarming(collector);
    }

    private void StopFarming(ICollector collector)
    {
        _isFarmingActive = false;
        _farmSequence.Kill();
        _fillImage.DOFillAmount(0, _farmCancelSpeed);

        if (CollectAnimation)
            collector.EndCollectAnimation();
    }

    private void SetCollectorResource(ICollector collector, GameObject interactor)
    {
        if (LimitCapacity)
        {
            if (_resourceAmount >= _amountPerTick)
            {
                _resourceAmount -= _amountPerTick;
                collector.AddResource(_amountPerTick, _resourceType);

                if (UseResourceModel && interactor.TryGetComponent(out ResourceContainer3D resourceContainer3D))
                    SpawnResourceModel(resourceContainer3D);

                if (ResourceToUIAnim)
                    SpawnResourceVisual();

            }
            else
            {
                StopFarming(collector);
            }
        }
        else
        {
            collector.AddResource(_amountPerTick, _resourceType);

            if (UseResourceModel && interactor.TryGetComponent(out ResourceContainer3D resourceContainer3D))
                SpawnResourceModel(resourceContainer3D);

            if (ResourceToUIAnim)
                SpawnResourceVisual();
        }
    }

    private void StartFarming(ICollector collector, GameObject interactor)
    {
        if (_isFarmingActive)
        {
            _farmSequence = DOTween.Sequence();
            _farmSequence.Append(_fillImage.DOFillAmount(1, _farmSpeed).OnComplete(() =>
            {
                SetCollectorResource(collector, interactor);
                ResetFill();
                StartFarming(collector, interactor);
            }));
        }
    }
    private void SpawnResourceVisual()
    {
        var resourceVisual = Instantiate(_resourceVisual, gameObject.transform);

        _resourceSequence = DOTween.Sequence();
        _resourceSequence.Append(resourceVisual.transform.DOMove(new Vector3(resourceVisual.gameObject.transform.position.x, resourceVisual.gameObject.transform.position.y + 2, resourceVisual.gameObject.transform.position.z), 0.4f))
            .Join(resourceVisual.gameObject.transform.DOScale(30f, 0.8f))
            .Append(resourceVisual.gameObject.transform.DOScale(0f, 0.3f))
            .OnComplete(() => Destroy(resourceVisual));
    }

    private void SpawnResourceModel(ResourceContainer3D resourceContainer3D)
    {
        var resource3D = Instantiate(_resourceModel, gameObject.transform.position, Quaternion.identity);
        resource3D.transform.localScale = new Vector3(0,0,0);
        resource3D.transform.DOScale(1, 0.1f);
        resourceContainer3D.Add(resource3D);
        

    }
    private void ChangeFillColor()
    {
        _fillImage.color = _fillColor;
    }
    private void ResetFill()
    {
        _fillImage.fillAmount = 0;
    }
}