using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
      _button = GetComponent<Button>();
      _button.onClick.AddListener(StartButtonEvent);
    }

    private void StartButtonEvent()
    {
        Sequence buttonSequence = DOTween.Sequence();
        buttonSequence.Append(transform.DOScale(0.6f, 0.1f)).Append(transform.DOScale(1f, 0.1f));
    }
}
