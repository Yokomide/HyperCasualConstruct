using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimationContoller : MonoBehaviour
{
    public Action OnAnimationEnd;
    private bool _callAnimationEndEvent;

    [TableList(ShowIndexLabels = true)]
    public List<AnimationSettings> _settings = new List<AnimationSettings>();

    [Serializable]
    public class AnimationSettings
    {
        public AnimationType _animationType;
        public Transform _targetPosition;
        public Vector3 _scaleTarget;

        public float _duration;
        public float _jumpForce;
        public int _numJumps;

    }
    public void StartAnimation(GameObject animationTarget, int index)
    {
        switch (_settings[index]._animationType)
        {
            case AnimationType.ScaleFromZero:
                ScaleFromZero(animationTarget, index);
                break;

            case AnimationType.ScaleToZero:
                ScaleToZero(animationTarget, index);
                break;

            case AnimationType.ScaleFromCurrent:
                ScaleFromCurrent(animationTarget, index);
                break;

            case AnimationType.Move:
                Move(animationTarget, index);
                break;

            case AnimationType.Jump:
                Jump(animationTarget, index);
                break;

        }
    }
    public void StartAnimation(GameObject animationTarget, AnimationType animationType, bool CallAnimationEndEvent, int index)
    {
        _callAnimationEndEvent = CallAnimationEndEvent;
        switch (animationType)
        {
            case AnimationType.ScaleFromZero:
                ScaleFromZero(animationTarget, index);
                break;

            case AnimationType.ScaleToZero:
                ScaleToZero(animationTarget, index);
                break;

            case AnimationType.ScaleFromCurrent:
                ScaleFromCurrent(animationTarget, index);
                break;

            case AnimationType.Move:
                Move(animationTarget, index);
                break;

            case AnimationType.Jump:
                Jump(animationTarget, index);
                break;

        }
    }
    private void ScaleToZero(GameObject animationTarget, int index)
    {
        animationTarget.transform.DOScale(Vector3.zero, _settings[index]._duration)
            .OnComplete(() =>
            {
                animationTarget.SetActive(false);
                if(_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
            });

    }
    private void ScaleFromZero(GameObject animationTarget, int index)
    {
        animationTarget.transform.localScale = Vector3.zero;
        animationTarget.transform.DOScale(_settings[index]._scaleTarget, _settings[index]._duration).OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
        });
    }
    private void ScaleFromCurrent(GameObject animationTarget, int index)
    {
        animationTarget.transform.DOScale(_settings[index]._scaleTarget, _settings[index]._duration).OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
        });
    }
    private void Move(GameObject animationTarget, int index)
    {
        animationTarget.transform.DOMove(_settings[index]._targetPosition.position, _settings[index]._duration).OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
        });
    }
    private void Jump(GameObject animationTarget, int index)
    {
        animationTarget.transform.DOJump(_settings[index]._targetPosition.position , _settings[index]._jumpForce, _settings[index]._numJumps, _settings[index]._duration).OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
        });
    }

}
