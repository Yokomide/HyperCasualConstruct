using System;

public interface IEventActivator
{
    public event Action OnEventActivate;
    public void ActivateEvent();
}