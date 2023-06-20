using System;

interface IEventActivator
{
    public event Action OnEventActivate;
    public void ActivateEvent();
}