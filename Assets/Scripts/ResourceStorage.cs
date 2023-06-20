using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "New Player Resources", menuName = "Player/Resources/Storage")]
public class ResourceStorage : ScriptableObject, IStorage
{
    [SerializeField] private int _goldAmount;
    [SerializeField] private int _woodAmount;
    [SerializeField] private int _blueDiamondAmount;
    [SerializeField] private int _redDiamondAmount;

    public int GoldAmount
    {
        get { return _goldAmount; }
        set { _goldAmount = value; }
    }

    public int BlueDiamondAmount
    {
        get { return _blueDiamondAmount; }
        set { _blueDiamondAmount = value; }
    }

    public int RedDiamondAmount
    {
        get { return _redDiamondAmount; }
        set { _redDiamondAmount = value; }
    }

    public int WoodAmount
    {
        get { return _woodAmount; }
        set { _woodAmount = value; }
    }

    public event Action OnValueChanged;

    public int GetAmount(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Gold:
                return GoldAmount;

            case ResourceType.BlueDiamond:
                return BlueDiamondAmount;   

            case ResourceType.RedDiamond:
                return RedDiamondAmount;

            case ResourceType.Wood:
                return WoodAmount;

            default: return 0;
        }
    }
    public void AddValue(int value, ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Gold:
                GoldAmount += value;
                break;

            case ResourceType.BlueDiamond:
                BlueDiamondAmount += value;
                break;

            case ResourceType.RedDiamond:
                RedDiamondAmount += value;
                break;

            case ResourceType.Wood:
                WoodAmount += value;
                break;

            default: break;
        }
        OnValueChanged?.Invoke();
    }

    public void RemoveValue(int value, ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Gold:
                GoldAmount -= value;
                break;

            case ResourceType.BlueDiamond:
                BlueDiamondAmount -= value;
                break;

            case ResourceType.RedDiamond:
                RedDiamondAmount -= value;
                break;

            case ResourceType.Wood:
                WoodAmount -= value;
                break;

            default: break;
        }
        OnValueChanged?.Invoke();
    }

    public void ResetValue(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Gold:
                GoldAmount = 0;
                break;

            case ResourceType.BlueDiamond:
                BlueDiamondAmount = 0;
                break;

            case ResourceType.RedDiamond:
                RedDiamondAmount = 0;
                break;

            case ResourceType.Wood:
                WoodAmount = 0;
                break;

            default: break;
        }
        OnValueChanged?.Invoke();
    }
    public int GetAllValuesSum()
    {
        return GoldAmount + BlueDiamondAmount + RedDiamondAmount + WoodAmount;
    }
    public void ResetAllValues()
    {
        GoldAmount = 0;
        BlueDiamondAmount = 0;
        RedDiamondAmount = 0;
        WoodAmount = 0;
        OnValueChanged?.Invoke();

    }
}