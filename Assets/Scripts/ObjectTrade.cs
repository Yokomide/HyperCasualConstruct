using DG.Tweening;
using Lofelt.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectTrade : MonoBehaviour
{
    [Header("====Resource Settings====")]
    //[SerializeField] private List<ResourceType> _requiredResources = new List<ResourceType>();
    [SerializeField] public RequiredResourcesData _requiredResources;
    [SerializeField] private GameObject _receivedResourcePrefab;

    [SerializeField] private int _maxAvailableSlots;
    [SerializeField] private bool _isOneToOne;
    private int _availableSlots;


    [Space(10)]

    [Header("====Trade Settings====")]
    [SerializeField] private float _tradeDelay;
    [SerializeField] private float _tradeSpeed;
    [Space(5)]
    [SerializeField] private float _spendJumpPower;
    [SerializeField] private float _spendDuration;
    [SerializeField] private bool _isTradingLocked;
    [SerializeField] private int _amountPerTick;
    [Space(10)]

    [Header("====Animation====")]
    [SerializeField] private bool TradeAnimation;
    [SerializeField] private CharacterAnimationType _characterAnimationType;
    [SerializeField] private float _animationSpeed;
    [Space(10)]

    [Header("====Visual====")]
    //[OnValueChanged("ChangeFillColor")]
    [SerializeField] private Transform _UIHolder;
    [SerializeField] private GameObject _UIPrefab;
    [SerializeField] public Transform resourceTarget;

    [HideInInspector] public event Action OnEventActivate;

    [Header("====Feelings====")]
    public HapticClip haptic;


    [HideInInspector] public Vector3 startResourceTargetScale;

    [SerializeField] private List<Transform> _targetTradePlace = new List<Transform>();

    private Dictionary<ResourceType, int> resourceStorage = new Dictionary<ResourceType, int>();

     private List<Resource3D> containerRequiredResources = new List<Resource3D>();
     private List<Resource3D> readyReceiveResources = new List<Resource3D>();


    private ICollector _collector;
    private ResourceContainer3D _container3D;

    private bool _isTradeActive;

    private bool _isCollectorIn;
    private bool _isGiving;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _collector = collector;
        _isCollectorIn = true;
        if (_requiredResources == null)
            return;
        _isTradeActive = true;
        if (TradeAnimation)
            collector.StartAnimation(_characterAnimationType, _animationSpeed);
        _container3D = other.GetComponent<ResourceContainer3D>();
        if (readyReceiveResources.Count > 0)
        {
            StartCoroutine(StartResourcesGiving());
        }
        StartCoroutine(PlaceTradeItem());
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _collector = null;
        _isCollectorIn = false;
    }

   IEnumerator StartResourcesGiving()
    {
        _isGiving = true;
        for (int i = 0; i <= readyReceiveResources.Count - 1; i++)
        {
            _container3D.Add(readyReceiveResources[i]);
            yield return new WaitForSeconds(0.4f);
        }
        readyReceiveResources.Clear();
        yield return new WaitForSeconds(0.5f);
        _isGiving = false;

    }
    private void Start()
    {
        SetRequirments(_requiredResources);
    }
    public void SetRequirments(RequiredResourcesData requiredResourcesData)
    {
        ClearDictionaries();
        _requiredResources = requiredResourcesData;
        foreach (RequiredResourcesData.ResourceRequirement requirement in requiredResourcesData.requiredResources)
        {
            resourceStorage.Add(requirement.type, 0);

            //var requirementUI = Instantiate(_UIPrefab, _UIHolder);
            //requirementUI.GetComponent<VisualRequireSetter>().Initialize(requirement.type, requirement.amount);
            //requireVisual.Add(requirement.type, requirementUI);
        }
        //_fillOnePercent = 1 / _fillMaxAmount;
        //_fillMaxAmount = 0;

    }
    IEnumerator PlaceTradeItem()
    {
        yield return new WaitUntil(() => !_isGiving);


        foreach (RequiredResourcesData.ResourceRequirement requirement in _requiredResources.requiredResources)
        {
            for (int i = _container3D.resourceAmount-1; i >= 0; i--)
            {
                if (_isOneToOne)
                {
                    if (_container3D.resources[i].Type == requirement.type && resourceStorage[requirement.type] < _maxAvailableSlots && containerRequiredResources.Count<_maxAvailableSlots)
                    {
                        containerRequiredResources.Add(_container3D.resources[i]);
                    }
                }
                else
                {
                    if (_container3D.resources[i].Type == requirement.type && resourceStorage[requirement.type] < requirement.amount)
                    {

                        containerRequiredResources.Add(_container3D.resources[i]);

                    }
                    else if (i == _container3D.resourceAmount - 1 && containerRequiredResources.Count <= 0)
                    {
                        if (_requiredResources.requiredResources.IndexOf(requirement) == _requiredResources.requiredResources.Count - 1)
                        {
                            //StopSpending();
                            yield break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }


            }
            for (int i = 0; i < containerRequiredResources.Count; i++)
            {
                if (_isOneToOne)
                {
                    if (resourceStorage[requirement.type] >= _maxAvailableSlots)
                    {
                        containerRequiredResources.Clear();
                        yield break;
                    }
                }
                else
                {
                    if (resourceStorage[requirement.type] >= requirement.amount)
                    {
                        containerRequiredResources.Clear();
                        break;
                    }
                }

                var spendSequence = DOTween.Sequence();
                spendSequence.SetAutoKill(false);
 
                Resource3D resource = containerRequiredResources[i];
                AddToStorage(_amountPerTick, requirement.type);
                resource.transform.parent = _targetTradePlace[resourceStorage[requirement.type]-1].transform;
                _collector.RemoveResource(_amountPerTick, requirement.type);
                _container3D.Remove(containerRequiredResources[i]);
                spendSequence.Append(resource.transform.DOJump(_targetTradePlace[resourceStorage[requirement.type]-1].position, _spendJumpPower, 1, _spendDuration)
                    .SetAutoKill(false)
                .OnComplete(() =>
                {
                    //resource.transform.parent = _targetTradePlace[i].transform;
                }));
                if (!_isCollectorIn)
                {
                    containerRequiredResources.Clear();
                }

                yield return new WaitUntil(() => spendSequence.IsComplete());
                spendSequence.Kill();
                spendSequence = null;
                StartCoroutine(TradeProcessing(resource));
                if (!_isCollectorIn)
                {
                    containerRequiredResources.Clear();
                    break;
                }
            }
            if (!_isCollectorIn)
            {
                containerRequiredResources.Clear();
                if(resourceStorage.Count >= _maxAvailableSlots)
                {
                    SetRequirments(_requiredResources);
                }
                yield break;
            }
            Debug.Log(resourceStorage.Count);
            containerRequiredResources.Clear();
            SetRequirments(_requiredResources);
        }
    }
    IEnumerator TradeProcessing(Resource3D resource)
    {
        Tween tradeProcessTween = resource.transform.DOScale(0.5f, 0.4f).SetLoops(-1, LoopType.Yoyo);
        yield return new WaitForSeconds(4f/ _container3D.GetComponent<PlayerStats>().TradeSpeed);
        tradeProcessTween.Complete();
        tradeProcessTween.Kill();
        Destroy(resource.gameObject);

        var newResource = Instantiate(_receivedResourcePrefab, resource.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        if (_isCollectorIn)
        {
            _container3D.Add(newResource.GetComponent<Resource3D>());
        }
        else
        {
            readyReceiveResources.Add(newResource.GetComponent<Resource3D>());
        }


        yield return null;
    }
    private void AddToStorage(int amount, ResourceType type)
    {
        // VisualRequireSetter visualRequireSetter = requireVisual[type].GetComponent<VisualRequireSetter>();
        // visualRequireSetter.SetValue(visualRequireSetter.Value - amount);
        resourceStorage[type] += amount;
        if (haptic != null)
        {
            HapticController.Play(haptic);
        }
        /* else
         {
             if (CheckResourceCompletion())
             {
                 StopSpending();
                 _requiredResources = null;
                 ActivateEvent();
                 return;
             }
         }
        */
    }
    public void ClearDictionaries()
    {
        resourceStorage.Clear();
    }
}