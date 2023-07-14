using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float FarmSpeed => _farmSpeed;
    public float SpendSpeed => _spendSpeed;
    public float TradeSpeed => _tradeSpeed;

    public event Action OnValueChanged;



    [Header("====Resource Storage====")]
    [SerializeField] private ResourceStorage _storage;
    [Space(10)]

    [Header("====Speed====")]
    [SerializeField] private float _farmSpeed;
    [SerializeField] private float _spendSpeed;
    [SerializeField] private float _tradeSpeed;
    [Space(10)]

    [Header("====Upgrade Speed Multiplier====")]
    [SerializeField] private float _farmSpeedMultiplier;
    [SerializeField] private float _spendSpeedMultiplier;
    [SerializeField] private float _tradeSpeedMultiplier;
    [Space(10)]


    [Header("====Cost====")]
    [SerializeField] private int _upgradeCostMultiplier;

    [SerializeField] private int _farmUpgradeCost;
    [SerializeField] private int _spendUpgradeCost;
    [SerializeField] private int _tradeUpgradeCost;
    [Space(10)]

    [Header("====Cost Resource Type====")]
    [SerializeField] private ResourceType _farmUpgradeResourceType;
    [SerializeField] private ResourceType _spendUpgradeResourceType;
    [SerializeField] private ResourceType _tradeUpgradeResourceType;

    public float GetCost(PlayerStatType statType)
    {
        switch (statType)
        {
            case PlayerStatType.FarmSpeed:
                return _farmUpgradeCost;
            case PlayerStatType.SpendSpeed:
                return _spendUpgradeCost;
            case PlayerStatType.TradeSpeed:
                return _tradeUpgradeCost;
            default:
                return 1;
        }
    }

    public void UpgradeFarmSpeed()
    {
        if (_farmUpgradeCost > _storage.GetAmount(_farmUpgradeResourceType))
            return;
        _storage.RemoveValue(_farmUpgradeCost, _farmUpgradeResourceType);
        _farmSpeed *= _farmSpeedMultiplier;
        _farmUpgradeCost *= _upgradeCostMultiplier;
        OnValueChanged?.Invoke();
    }

    public void UpgradeSpendSpeed()
    {
        if (_spendUpgradeCost > _storage.GetAmount(_spendUpgradeResourceType))
            return;
        _storage.RemoveValue(_spendUpgradeCost, _spendUpgradeResourceType);
        _spendSpeed *= _spendSpeedMultiplier;
        _spendUpgradeCost *= _upgradeCostMultiplier;
        OnValueChanged?.Invoke();

    }

    public void UpgradeTradeSpeed()
    {
        if (_tradeUpgradeCost > _storage.GetAmount(_tradeUpgradeResourceType))
            return;
        _storage.RemoveValue(_tradeUpgradeCost, _tradeUpgradeResourceType);
        _tradeSpeed *= _tradeSpeedMultiplier;
        _tradeUpgradeCost *= _upgradeCostMultiplier;
        OnValueChanged?.Invoke();

    }
}