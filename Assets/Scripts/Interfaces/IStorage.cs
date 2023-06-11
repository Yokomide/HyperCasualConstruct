using System;

internal interface IStorage
{
    public event Action OnValueChanged;

    public void AddValue(int value, ResourceType type);

    public void RemoveValue(int value, ResourceType type);

    public void ResetValue();
}