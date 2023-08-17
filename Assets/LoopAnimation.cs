using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAnimation : MonoBehaviour
{
    [EnumPaging]
    public AnimationType animationType;

    [SerializeField] private bool _playOnStart;
    [SerializeField] private bool _staticAnimation;

    [SerializeField] private Vector3 _from;
    [SerializeField] private Vector3 _to;

    [SerializeField] private float _duration;


    private void Start()
    {
        if (_playOnStart)
            StartAnimation();
    }

    public void StartAnimation()
    {
        switch (animationType)
        {

            case AnimationType.Move:
                Move();
                break;
        }
    }
    public void Move()
    {
        transform.DOMove(transform.position + _to, _duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
