using System;

public interface ISpender
{
    public void StartSpending();
    public void StopSpending();
    public bool CheckResourceCompletion();
    public void AddToStorage(int amount, ResourceType type);

}