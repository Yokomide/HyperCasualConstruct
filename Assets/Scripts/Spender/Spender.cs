
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spender : MonoBehaviour, IEventActivator
{
    [Header("====Resource Settings====")]
    [SerializeField] protected bool _initializeRequrimentsOnStart;
    [SerializeField] public RequiredResourcesData _requiredResources;
    [Space(10)]

    [Header("====Spend Settings====")]

    [SerializeField] protected float _spendDelay;
    [SerializeField] protected float _spendSpeed;
    [SerializeField] protected float _spendCancelSpeed;
    [SerializeField] protected float _spendDuration;

    [SerializeField] protected bool _lockOnComplete;

    [SerializeField] protected bool _isSpendingLocked;
    [SerializeField] protected int _amountPerTick;

    [SerializeField] protected bool SpendAnimation;
    [SerializeField] protected CharacterAnimationType _characterAnimationType;
    [SerializeField] protected float _animationSpeed;

    [SerializeField] protected Transform _UIHolder;
    [SerializeField] protected GameObject _UIPrefab;
    [SerializeField] public Transform resourceTarget;
    [Space(10)]
    [Header("====Visual Settings====")]
    [OnValueChanged("ChangeFillColor")]
    [SerializeField] protected Color _fillColor;
    [SerializeField] protected Image _fillImage;
    protected float _fillMaxAmount;
    protected float _fillOnePercent;

    [Space(10)]
    [Header("====Feelings====")]
    public HapticClip haptic;

    protected bool _isSpendingActive;
    
    protected Dictionary<ResourceType, int> resourceStorage = new Dictionary<ResourceType, int>();

    protected Dictionary<ResourceType, GameObject> requireVisual = new Dictionary<ResourceType, GameObject>();
    [HideInInspector] public Vector3 startResourceTargetScale;
    public ICollector _collector;
    public event Action OnEventActivate;
    public abstract void ActivateSpender();

    public abstract void StartSpending();
    public abstract void StopSpending();
    public abstract bool CheckResourceCompletion();
    public abstract void AddToStorage(int amount, ResourceType type);
    // public abstract void UpdateRequirments(RequiredResourcesData requiredResourcesData);
    //  public abstract void ClearDictionaries();

    public void UpdateRequirments(RequiredResourcesData requiredResourcesData)
    {
        ClearDictionaries();
        _requiredResources = requiredResourcesData;
        foreach (RequiredResourcesData.ResourceRequirement requirement in requiredResourcesData.requiredResources)
        {
            resourceStorage.Add(requirement.type, 0);

            _fillMaxAmount += requirement.amount;

            if (_UIHolder != null & _UIPrefab != null)
            {
                var requirementUI = Instantiate(_UIPrefab, _UIHolder);
                requirementUI.GetComponent<VisualRequireSetter>().Initialize(requirement.type, requirement.amount);
                requireVisual.Add(requirement.type, requirementUI);
            }
        }
        _fillOnePercent = 1 / _fillMaxAmount;
        _fillMaxAmount = 0;

    }
    public bool CheckSpenderStatus()
    {
        if (_isSpendingLocked)
            return false;
        if (_requiredResources == null)
            return false;
        return true;
    }

    public void ClearDictionaries()
    {
        resourceStorage.Clear();
        if (_fillImage != null)
        {
            _fillImage.fillAmount = 0;
        }
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
    protected void ChangeFillColor()
    {
        _fillImage.color = _fillColor;
    }
    public void ActivateEvent()
    {
        
        OnEventActivate?.Invoke();
        if(_lockOnComplete)
        {
            LockSpend();
        }
    }
}

