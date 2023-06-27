using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static RequiredResourcesData;
using Sequence = DG.Tweening.Sequence;

public class ResourceSpender : MonoBehaviour, IEventActivator
{
    [Header("====Resource Settings====")]
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] public RequiredResourcesData  _requiredResources;


    [Space(10)]

    [Header("====Spend Settings====")]
    [SerializeField] private float _spendDelay;
    [SerializeField] private float _spendSpeed;
    [SerializeField] private float _spendCancelSpeed;
    [SerializeField] private float _spendJumpPower;
    [SerializeField] private float _spendDuration;
    [Space(5)]
    [SerializeField] private bool _isSpendingLocked;
    [SerializeField] private int _amountPerTick;
    [SerializeField] private bool _suckResource3D;
    [Space(10)]

    [Header("====Animation====")]
    [SerializeField] private bool SpendAnimation;
    [SerializeField] private CharacterAnimationType _characterAnimationType;
    [SerializeField] private float _animationSpeed;
    [Space(10)]

    [Header("====Visual====")]
    [OnValueChanged("ChangeFillColor")]
    [SerializeField] private Color _fillColor;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Transform _UIHolder;
    [SerializeField] private GameObject _UIPrefab;

    [HideInInspector] public event Action OnEventActivate;

    private Dictionary<ResourceType, int> resourceStorage = new Dictionary<ResourceType, int>();

    private Dictionary<ResourceType, GameObject> requireVisual = new Dictionary<ResourceType, GameObject>();

    private ResourceContainer3D _container3D;
    private Sequence _spendSequence;

    private float _fillMaxAmount;
    private float _fillOnePercent;

    private bool _isSpendingActive;

    private void OnTriggerEnter(Collider other)
    {
        if (_isSpendingLocked)
            return;
 
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _isSpendingActive = true;
        if (SpendAnimation)
            collector.StartAnimation(_characterAnimationType, _animationSpeed);
        if (_suckResource3D)
            _container3D = other.GetComponent<ResourceContainer3D>();
        StartCoroutine(StartSpendingWithDelay(collector, other.gameObject));
        // StartSpending(collector, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        StopSpending(collector);
    }
    public void UpdateRequirments(/*ResourceStorage resourceRequirments,*/ RequiredResourcesData requiredResourcesData)
    {
        //_resourceRequirements = resourceRequirments;
        ClearDictionaries();

        _requiredResources = requiredResourcesData;
        foreach (RequiredResourcesData.ResourceRequirement requirement in requiredResourcesData.requiredResources)
        {
            resourceStorage.Add(requirement.type, 0);

            _fillMaxAmount += requirement.amount;
            Debug.Log(_fillMaxAmount);
            var requirementUI = Instantiate(_UIPrefab, _UIHolder);
            requirementUI.GetComponent<VisualRequireSetter>().Initialize(requirement.type, requirement.amount);
            requireVisual.Add(requirement.type, requirementUI);
        }
       _fillOnePercent = 1 / _fillMaxAmount;
        _fillMaxAmount = 0;

    }
    private void StartSpending(ICollector collector, GameObject interactor)
    {
        if (_isSpendingLocked)
            return;
        if (!_isSpendingActive)
            return;
        //if (_resourceRequirements == null)
        //return;
        if (_suckResource3D)
        {
            _spendSequence = DOTween.Sequence();
            if (_container3D.resourceModelsAmount <= 0)
            {
                StopSpending(collector);
                return;
            }
                Debug.Log(_container3D.resourceModelsAmount - 1);
            var resourceModel = _container3D.resourceModels[_container3D.resourceModelsAmount - 1];
            _container3D.Remove(_container3D.resourceModels[_container3D.resourceModelsAmount - 1]);
            _spendSequence.Append(resourceModel.transform.DOJump(transform.position, _spendJumpPower, 1, _spendDuration)
                .OnComplete(() =>
            {
<<<<<<< HEAD
<<<<<<< Updated upstream
                Destroy(resourceModel);
                collector.RemoveResource(_amountPerTick, _resourceType);

            AddToStorage(_amountPerTick, ResourceType.Gold);
=======

                Destroy(resourceModel);
                collector.RemoveResource(_amountPerTick, ResourceType.Gold);
                AddToStorage(_amountPerTick, ResourceType.Gold);
>>>>>>> Stashed changes
=======

            collector.RemoveResource(_amountPerTick, _resourceType);

            AddToStorage(_amountPerTick, ResourceType.Gold);
            Destroy(resourceModel);
>>>>>>> parent of 3bc2940c (Update)
                /*
            if (_resourceStorage.GetAmount(ResourceType.Gold) >= _resourceRequirements.GetAmount(ResourceType.Gold))
            {
                _isSpendingLocked = true;
                StopSpending(collector);
                ActivateEvent();

                return;
            }
                */
                //StartSpending(collector, interactor);

            }));

        }
        else
        {
            int runOutResources = 0;
            foreach (RequiredResourcesData.ResourceRequirement requirement in _requiredResources.requiredResources)
            {

                if (!resourceStorage.ContainsKey(requirement.type))
                    continue;

                if (resourceStorage[requirement.type] < requirement.amount && collector.GetResourceAmount(requirement.type) >= _amountPerTick)
                {
                    collector.RemoveResource(_amountPerTick, requirement.type);
                    AddToStorage(_amountPerTick, requirement.type);
                }
                else
                {
                    runOutResources++;
                }
            }
            Debug.Log(runOutResources);
            if (CheckResourceCompletion())
            {
                StopSpending(collector);
                ActivateEvent();
                return;
            }
            if (runOutResources == _requiredResources.requiredResources.Count())
            {
                StopSpending(collector);
            }

        }
    }
    private bool CheckResourceCompletion()
    {
        foreach (RequiredResourcesData.ResourceRequirement requirement in _requiredResources.requiredResources)
        {
            if (resourceStorage[requirement.type] != requirement.amount)
            {
                return false;
            }
        }
        return true;
    }

    private void AddToStorage(int amount, ResourceType type)
    {
        VisualRequireSetter visualRequireSetter = requireVisual[type].GetComponent<VisualRequireSetter>();
        visualRequireSetter.SetValue(visualRequireSetter.Value-amount);
       _fillImage.fillAmount += _fillOnePercent * amount;
        resourceStorage[type] += amount;
        //_resourceStorage.AddValue(amount, type);
    }
    private void StopSpending(ICollector collector)
    {
        _isSpendingActive = false;
        if (SpendAnimation)
            collector.EndAnimation();
    }
    private void ChangeFillColor()
    {
        _fillImage.color = _fillColor;
    }
    public void ClearDictionaries()
    {
        resourceStorage.Clear();
        if (requireVisual.Count > 0)
        {
            foreach (GameObject visualRequire in requireVisual.Values)
            {
                Destroy(visualRequire);
            }
            requireVisual.Clear();
        }
    }
    public void LockSpend()
    {
        _isSpendingLocked = true;
    }
    public void UnlockSpend()
    {
        _isSpendingLocked = false;
    }
    public void ActivateEvent()
    {
        _fillImage.DOFillAmount(0, 0.1f);
        OnEventActivate?.Invoke();
    }
    IEnumerator StartSpendingWithDelay(ICollector collector, GameObject interactor)
    {
        while (_isSpendingActive && !_isSpendingLocked)
        {
            StartSpending(collector, interactor);
            yield return new WaitForSeconds(_spendDelay);
        }
        yield return null;
    }
}
