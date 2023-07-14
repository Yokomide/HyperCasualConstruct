
using Lofelt.NiceVibrations;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spender : MonoBehaviour, IEventActivator
{
    [Header("====Resource Settings====")]
    [SerializeField] public RequiredResourcesData _requiredResources;
    [Space(10)]

    [SerializeField] protected float _spendDelay;
    [SerializeField] protected float _spendSpeed;
    [SerializeField] protected float _spendCancelSpeed;
    [SerializeField] protected float _spendDuration;

    [SerializeField] protected bool _isSpendingLocked;
    [SerializeField] protected int _amountPerTick;

    [SerializeField] protected bool SpendAnimation;
    [SerializeField] protected CharacterAnimationType _characterAnimationType;
    [SerializeField] protected float _animationSpeed;

    [SerializeField] protected Transform _UIHolder;
    [SerializeField] protected GameObject _UIPrefab;
    [SerializeField] public Transform resourceTarget;

    [Space(10)]
    [Header("====Feelings====")]
    public HapticClip haptic;

    protected Dictionary<ResourceType, int> resourceStorage = new Dictionary<ResourceType, int>();

    protected Dictionary<ResourceType, GameObject> requireVisual = new Dictionary<ResourceType, GameObject>();
    [HideInInspector] public Vector3 startResourceTargetScale;
   // protected ICollector _collector;
    public event Action OnEventActivate;

    public abstract void StartSpending();
    public abstract void StopSpending();
    public abstract bool CheckResourceCompletion();
    public abstract void AddToStorage(int amount, ResourceType type);
    public abstract void UpdateRequirments(RequiredResourcesData requiredResourcesData);
    public abstract void ClearDictionaries();

    public void ActivateEvent()
    {
        OnEventActivate?.Invoke();
    }
}

