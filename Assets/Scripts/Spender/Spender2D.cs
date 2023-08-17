using DG.Tweening;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Spender2D : Spender
{
    private void Awake()
    {
        if (_initializeRequrimentsOnStart)
        {
            UpdateRequirments(_requiredResources);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _collector = collector;
        ActivateSpender();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        StopSpending();
    }
    public override void ActivateSpender()
    {
        if (!CheckSpenderStatus())
            return;
        _isSpendingActive = true;
        if (SpendAnimation)
            _collector.StartAnimation(_characterAnimationType, _animationSpeed);
        StartSpending();

    }
    public override void StartSpending()
    {
        StartCoroutine(StartSpendingWithDelay());
    }

    private void Spend()
    {
        if (_isSpendingLocked)
            return;
        if (!_isSpendingActive)
            return;
        int runOutResources = 0;
        foreach (RequiredResourcesData.ResourceRequirement requirement in _requiredResources.requiredResources)
        {

            if (!resourceStorage.ContainsKey(requirement.type))
                continue;

            if (resourceStorage[requirement.type] < requirement.amount && _collector.GetResourceAmount(requirement.type) >= _amountPerTick)
            {
                _collector.RemoveResource(_amountPerTick, requirement.type);
                AddToStorage(_amountPerTick, requirement.type);
            }
            else
            {
                runOutResources++;
            }
        }
        if (runOutResources == _requiredResources.requiredResources.Count())
        {
            StopSpending();
        }
    }

    public override void StopSpending()
    {
        _isSpendingActive = false;
        if (SpendAnimation)
        {
            if (_collector != null)
            {
                _collector.EndAnimation();
            }
        }
        _collector = null;
    }
    public override void AddToStorage(int amount, ResourceType type)
    {
        if (requireVisual.Count > 0)
        {
            Debug.Log(requireVisual[type]);
            VisualRequireSetter visualRequireSetter = requireVisual[type].GetComponent<VisualRequireSetter>();
            visualRequireSetter.SetValue(visualRequireSetter.Value - amount);
        }
        if (_fillImage != null)
        {
            _fillImage.fillAmount += _fillOnePercent * amount;
        }
        resourceStorage[type] += amount;
        if (haptic != null)
        {
            HapticController.Play(haptic);
        }
        if (CheckResourceCompletion())
        {
            StopSpending();
            _requiredResources = null;
            ActivateEvent();
            if (_initializeRequrimentsOnStart)
                UpdateRequirments(_requiredResources);
            return;
        }
    }

    IEnumerator StartSpendingWithDelay()
    {
        while (_isSpendingActive && !_isSpendingLocked)
        {
            Spend();
            yield return new WaitForSeconds(_spendDelay);
        }
        yield return null;
    }
    public override bool CheckResourceCompletion()
    {
        foreach (RequiredResourcesData.ResourceRequirement requirement in _requiredResources.requiredResources)
        {
            if (resourceStorage[requirement.type] != requirement.amount)
            {
                return false;
            }
        }
        return true;
    }

}
