using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class ResourceSpender : MonoBehaviour, IEventActivator
{
    [Header("====Resource Settings====")]
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] public ResourceStorage _resourceRequirements;

    [Space(10)]

    [Header("====Spend Settings====")]
    [SerializeField] public int _resourceRequired;
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
    [Space(10)]

    [Header("====Visual====")]
    [OnValueChanged("ChangeFillColor")]
    [SerializeField] private Color _fillColor;
    [SerializeField] private Image _fillImage;

    [HideInInspector] public event Action OnEventActivate;

    private ResourceContainer3D _container3D;
    private Sequence _spendSequence;

    private float _fillMaxAmount;
    private float _fillOnePercent;

    private bool _isSpendingActive;

    private void OnTriggerEnter(Collider other)
    {
        if (_isSpendingLocked)
            return;
        if (_resourceRequirements == null)
            return;
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _isSpendingActive = true;
        if (_suckResource3D)
            _container3D = other.GetComponent<ResourceContainer3D>();
        StartCoroutine(StartSpendingWithDelay(collector, other.gameObject));
       // StartSpending(collector, other.gameObject);

        if (SpendAnimation)
            collector.StartAnimation(_characterAnimationType);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        StopSpending(collector);
    }
    public void UpdateRequirments(ResourceStorage resourceRequirments)
    {
        _resourceRequirements = resourceRequirments;
        _fillMaxAmount = _resourceRequirements.GetAllValuesSum();
        _fillOnePercent = 1 / _fillMaxAmount;

    }
    private void StartSpending(ICollector collector, GameObject interactor)
    {
        if (_isSpendingLocked)
            return;
        if (!_isSpendingActive)
            return;
        if (_resourceRequirements == null)
            return;
        if (_suckResource3D)
        {
            _spendSequence = DOTween.Sequence();
            if (_container3D.resourceModels.Count <= 0)
                StopSpending(collector);

            var resourceModel = _container3D.resourceModels[_container3D.resourceModelsAmount - 1];
            _spendSequence.Append(_container3D.resourceModels[_container3D.resourceModelsAmount - 1].transform.DOJump(transform.position, _spendJumpPower, 1, _spendDuration)
                .OnComplete(() =>
        {
            _container3D.Remove(_container3D.resourceModels[_container3D.resourceModelsAmount - 1]);

            Destroy(resourceModel);

            collector.RemoveResource(_amountPerTick, _resourceType);

            AddToStorage(_amountPerTick, _resourceType);

            if (_resourceStorage.GetAmount(_resourceType) >= _resourceRequired)
            {
                _isSpendingLocked = true;
                StopSpending(collector);
                ActivateEvent();

                return;
            }
            StartSpending(collector, interactor);

        }));

        }
        else
        {
            if(_resourceRequirements.GetAmount(ResourceType.Gold) > 0 && _resourceRequirements.GetAmount(ResourceType.Gold) != _resourceStorage.GetAmount(ResourceType.Gold) && collector.GetResourceAmount(ResourceType.Gold) >= _amountPerTick)
            { 
            collector.RemoveResource(_amountPerTick, ResourceType.Gold);
            AddToStorage(_amountPerTick,ResourceType.Gold);
            }
            if (_resourceRequirements.GetAmount(ResourceType.BlueDiamond) > 0 && _resourceRequirements.GetAmount(ResourceType.BlueDiamond) != _resourceStorage.GetAmount(ResourceType.BlueDiamond) && collector.GetResourceAmount(ResourceType.BlueDiamond) >= _amountPerTick)
            {
                collector.RemoveResource(_amountPerTick, ResourceType.BlueDiamond );
                AddToStorage(_amountPerTick, ResourceType.BlueDiamond);
            }
            if (_resourceRequirements.GetAmount(ResourceType.RedDiamond) > 0 && _resourceRequirements.GetAmount(ResourceType.RedDiamond) != _resourceStorage.GetAmount(ResourceType.RedDiamond) && collector.GetResourceAmount(ResourceType.RedDiamond) >= _amountPerTick)
            {
                collector.RemoveResource(_amountPerTick, ResourceType.RedDiamond);
                AddToStorage(_amountPerTick, ResourceType.RedDiamond);
            }
            if (_resourceRequirements.GetAmount(ResourceType.Wood) > 0 && _resourceRequirements.GetAmount(ResourceType.Wood) != _resourceStorage.GetAmount(ResourceType.Wood) && collector.GetResourceAmount(ResourceType.Wood) >= _amountPerTick)
            {
                collector.RemoveResource(_amountPerTick, ResourceType.Wood);
                AddToStorage(_amountPerTick, ResourceType.Wood);
            }
            if(_resourceRequirements.GetAmount(ResourceType.Gold) == _resourceStorage.GetAmount(ResourceType.Gold) &&
                _resourceRequirements.GetAmount(ResourceType.BlueDiamond) == _resourceStorage.GetAmount(ResourceType.BlueDiamond) &&
                _resourceRequirements.GetAmount(ResourceType.RedDiamond) == _resourceStorage.GetAmount(ResourceType.RedDiamond) &&
                    _resourceRequirements.GetAmount(ResourceType.Wood) == _resourceStorage.GetAmount(ResourceType.Wood))
            {
                _isSpendingLocked = true;
                StopSpending(collector);
                ActivateEvent();

                return;
            }

        }
    }
    private void AddToStorage(int amount, ResourceType type)
    {
        _fillImage.fillAmount += _fillOnePercent * amount;
        _resourceStorage.AddValue(amount, type);
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
    public void ResetResourceAmount()
    {
        _resourceStorage.ResetAllValues();
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
    {  while (_isSpendingActive && !_isSpendingLocked)
        {
            StartSpending(collector, interactor);
            yield return new WaitForSeconds(_spendDelay);
        }
        yield return null;
    }
}
