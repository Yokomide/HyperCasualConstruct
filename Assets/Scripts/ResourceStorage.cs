using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Resources", menuName = "Player/Resources/Storage")]
public class ResourceStorage : ScriptableObject, IStorage
{
    public event Action OnValueChanged;

    [SerializeField] private int _goldAmount;
    [SerializeField] private int _blueDiamondAmount;
    [SerializeField] private int _redDiamondAmount;
    [SerializeField] private int _burgerAmount;
    [SerializeField] private int _bunAmount;
    [SerializeField] private int _cutletAmount;

    private ref int GetResourceReference(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Gold:
                return ref _goldAmount;

            case ResourceType.BlueDiamond:
                return ref _blueDiamondAmount;

            case ResourceType.RedDiamond:
                return ref _redDiamondAmount;

            case ResourceType.Burger:
                return ref _burgerAmount;

            case ResourceType.Bun:
                return ref _bunAmount;

            case ResourceType.Cutlet:
                return ref _cutletAmount;

            default: return ref _goldAmount;
        }
    }

    public int GetAmount(ResourceType type)
    {
       return GetResourceReference(type);

    }
    public void AddValue(int value, ResourceType type)
    {
        GetResourceReference(type) += value;
        OnValueChanged?.Invoke();
    }

    public void RemoveValue(int value, ResourceType type)
    {
        GetResourceReference(type) -= value;
        OnValueChanged?.Invoke();
    }

    public void ResetValue(ResourceType type)
    {
        GetResourceReference(type) = 0;
        OnValueChanged?.Invoke();
    }
}