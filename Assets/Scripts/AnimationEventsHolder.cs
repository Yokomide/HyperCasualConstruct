using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class AnimationEventsHolder : MonoBehaviour
{
    public event Action OnAnimationAction;
    public event Action OnAnimationEnd;
    public event Action OnAnimationLook;
    public void InvokeAction()
    {
        OnAnimationAction?.Invoke();
    }
    public void InvokeAnimationEnd()
    {
        OnAnimationEnd?.Invoke();
    }
    public void InvokeAnimationLook()
    {
        OnAnimationLook?.Invoke();
    }
}
