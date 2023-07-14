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

    public Dictionary<int, Tween> _storedAnimation = new Dictionary<int, Tween>();
    public Dictionary<int, GameObject> _animationTargets = new Dictionary<int, GameObject>();


    [Serializable]
    public class AnimationSettings
    {
        [EnumPaging]
        public AnimationType animationType;

        public float _duration;
        [ShowIf("@animationType == AnimationType.Move || animationType == AnimationType.Jump")]
        public Transform _targetPosition;
        [ShowIf("@animationType == AnimationType.ScaleFromZero || animationType == AnimationType.ScaleToZero || animationType == AnimationType.ScaleFromCurrent")]
        public Vector3 _scaleTarget;
        [ShowIf("animationType", AnimationType.Jump)]
        public float _jumpForce;
        [ShowIf("animationType", AnimationType.Jump)]
        public int _numJumps;
        //public bool _loop;

        public bool _storeAnimation;


    }
    public void StartAnimation(GameObject animationTarget, int index)
    {
        switch (_settings[index].animationType)
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

    public void RewindAnimation(int index)
    {
        if (_storedAnimation.Count <= 0)
        {
            return;
        }
        if (_storedAnimation[index] == null)
        {
            return;
        }
        _storedAnimation[index].SmoothRewind();
        _storedAnimation[index].OnRewind(() =>

        {
            _animationTargets[index].SetActive(false);
            ClearStoredElement(index);
        });
    }
    private void ScaleToZero(GameObject animationTarget, int index)
    {
        var animation = animationTarget.transform.DOScale(Vector3.zero, _settings[index]._duration)
             .SetAutoKill(false);

        animation.OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
            if (!_settings[index]._storeAnimation)
            {
                animation.Kill();
            }
        });
        if (_settings[index]._storeAnimation)
        {
            _storedAnimation.Add(index, animation);

            _animationTargets.Add(index, animationTarget);
        }

    }
    private void ScaleFromZero(GameObject animationTarget, int index)
    {
        animationTarget.transform.localScale = Vector3.zero;
        var animation = animationTarget.transform.DOScale(_settings[index]._scaleTarget, _settings[index]._duration)
              .SetAutoKill(false);

        animation.OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
            if (!_settings[index]._storeAnimation)
            {
                animation.Kill();
            }
        });
        if (_settings[index]._storeAnimation)
        {
            _storedAnimation.Add(index, animation);

            _animationTargets.Add(index, animationTarget);
        }
    }
    private void ScaleFromCurrent(GameObject animationTarget, int index)
    {
        var animation = animationTarget.transform.DOScale(_settings[index]._scaleTarget, _settings[index]._duration)
             .SetAutoKill(false);

        animation.OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
            if (!_settings[index]._storeAnimation)
            {
                animation.Kill();
            }
        });
        if (_settings[index]._storeAnimation)
        {
            _storedAnimation.Add(index, animation);

            _animationTargets.Add(index, animationTarget);
        }
    }
    private void Move(GameObject animationTarget, int index)
    {
        if (_storedAnimation.Count > 0)
        {
            if (_storedAnimation[index] != null)
            {
                if (_storedAnimation[index].IsPlaying())
                {

                    return;
                }
            }
        }
        var animation = animationTarget.transform.DOMove(_settings[index]._targetPosition.position, _settings[index]._duration)
             .SetAutoKill(false);

        animation.OnComplete(() =>
         {
             if (_callAnimationEndEvent)
                 OnAnimationEnd?.Invoke();
             if (!_settings[index]._storeAnimation)
             {
                 animation.Kill();
             }
         });
        if (_settings[index]._storeAnimation)
        {
            Debug.Log("Должно добавить");
            _storedAnimation.Add(index, animation);

            _animationTargets.Add(index, animationTarget);
        }
    }
    private void Jump(GameObject animationTarget, int index)
    {
        var animation = animationTarget.transform.DOJump(_settings[index]._targetPosition.position, _settings[index]._jumpForce, _settings[index]._numJumps, _settings[index]._duration)
             .SetAutoKill(false);

        animation.OnComplete(() =>
        {
            if (_callAnimationEndEvent)
                OnAnimationEnd?.Invoke();
            if (!_settings[index]._storeAnimation)
            {
                animation.Kill();
            }
        });
        if (_settings[index]._storeAnimation)
        {
            _storedAnimation.Add(index, animation);

            _animationTargets.Add(index, animationTarget);
        }
    }

    public void ClearStored()
    {
        _storedAnimation.Clear();
        _animationTargets.Clear();
    }


    public void ClearStoredElement(int Index)
    {
        _storedAnimation.Remove(Index);
        _animationTargets.Remove(Index);
    }

}
