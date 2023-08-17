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
<<<<<<< Updated upstream:Assets/Scripts/ResourceStorage.cs

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
=======
    [SerializeField] private int _burgerAmount;
    [SerializeField] private int _bunAmount;
    [SerializeField] private int _cutletAmount;
    [SerializeField] private int _rawCutletAmount;
    [SerializeField] private int _paperAmount;
    [SerializeField] private int _fleshAmount;
    [SerializeField] private int _boneAmount;
    [SerializeField] private int _essenceAmount;




    private ref int GetResourceReference(ResourceType type)
>>>>>>> Stashed changes:Assets/Scripts/Resource/ResourceStorage.cs
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

<<<<<<< Updated upstream:Assets/Scripts/ResourceStorage.cs
            default: return 0;
=======
            case ResourceType.Bun:
                return ref _bunAmount;

            case ResourceType.Cutlet:
                return ref _cutletAmount;

            case ResourceType.RawCutlet:
                return ref _rawCutletAmount;

            case ResourceType.Paper:
                return ref _paperAmount;

            case ResourceType.Flesh:
                return ref _fleshAmount;

            case ResourceType.Essence:
                return ref _essenceAmount;

            case ResourceType.Bone:
                return ref _boneAmount;

            default: return ref _goldAmount;
>>>>>>> Stashed changes:Assets/Scripts/Resource/ResourceStorage.cs
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
<<<<<<< Updated upstream:Assets/Scripts/ResourceStorage.cs
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
=======
        var reference = GetResourceReference(type) -= value;
        if(reference<0)
        {
            reference = 0;
>>>>>>> Stashed changes:Assets/Scripts/Resource/ResourceStorage.cs
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