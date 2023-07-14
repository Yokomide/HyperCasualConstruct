using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerStatsValueUpdater : MonoBehaviour
{
    [SerializeField] private PlayerStatType _statType;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _playerStats.OnValueChanged += SetValue;
    }

    private void OnDisable()
    {
        _playerStats.OnValueChanged -= SetValue;
    }

    private void Start()
    {
        SetValue();
    }

    private void SetValue()
    {
        _text.text = _playerStats.GetCost(_statType).ToString();
    }
}
