using DG.Tweening;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Spender3D : Spender
{
    [Space(10)]

    [Header("====Spend Settings====")]
    [SerializeField] private float _spendJumpPower;

    [Header("====Visual====")]
    [OnValueChanged("ChangeFillColor")]
    [SerializeField] private Color _fillColor;
    [SerializeField] private Image _fillImage;

    private ICollector _collector;

    private List<Resource3D> containerRequiredResources = new List<Resource3D>();

    private ResourceContainer3D _container3D;

    private float _fillMaxAmount;
    private float _fillOnePercent;

    private bool _isSpendingActive;

    private Coroutine spending3DCoroutine;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _collector = collector;
        if (_isSpendingLocked)
            return;
        if (_requiredResources == null)
            return;
        _isSpendingActive = true;
        if (SpendAnimation)
            collector.StartAnimation(_characterAnimationType, _animationSpeed);

        _container3D = other.GetComponent<ResourceContainer3D>();
        StartSpending();
        return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        StopSpending();
    }

    public override void UpdateRequirments(RequiredResourcesData requiredResourcesData)
    {
        ClearDictionaries();
        _requiredResources = requiredResourcesData;
        foreach (RequiredResourcesData.ResourceRequirement requirement in requiredResourcesData.requiredResources)
        {
            resourceStorage.Add(requirement.type, 0);

            _fillMaxAmount += requirement.amount;
            var requirementUI = Instantiate(_UIPrefab, _UIHolder);
            requirementUI.GetComponent<VisualRequireSetter>().Initialize(requirement.type, requirement.amount);
            requireVisual.Add(requirement.type, requirementUI);
        }
        _fillOnePercent = 1 / _fillMaxAmount;
        _fillMaxAmount = 0;

    }


    public override void StartSpending()
    {
        spending3DCoroutine = StartCoroutine(StartSpending3D());
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
        containerRequiredResources.Clear();
        if (spending3DCoroutine != null)
        {
            StopCoroutine(spending3DCoroutine);
            spending3DCoroutine = null;
        }
        _collector = null;
    }

    IEnumerator StartSpending3D()
    {
        if (_isSpendingLocked)
            yield break;
        if (!_isSpendingActive)
            yield break;
        if (_container3D.resourceAmount <= 0)
        {
            StopSpending();
            yield break;
        }
        foreach (RequiredResourcesData.ResourceRequirement requirement in _requiredResources.requiredResources)
        {
            for (int i = 0; i < _container3D.resourceAmount; i++)
            {

                if (_container3D.resources[i].Type == requirement.type && resourceStorage[requirement.type] < requirement.amount)
                {

                    containerRequiredResources.Add(_container3D.resources[i]);

                }
                else if (i == _container3D.resourceAmount - 1 && containerRequiredResources.Count <= 0)
                {
                    if (_requiredResources.requiredResources.IndexOf(requirement) == _requiredResources.requiredResources.Count - 1)
                    {
                        StopSpending();
                        yield break;
                    }
                    else
                    {
                        continue;
                    }
                }

            }
            for (int i = containerRequiredResources.Count - 1; i >= 0; i--)
            {
                if (resourceStorage[requirement.type] >= requirement.amount)
                {
                    containerRequiredResources.Clear();
                    break;

                }
                var targetStartScale = resourceTarget.localScale;
                var spendSequence = DOTween.Sequence();
                spendSequence.SetAutoKill(false);
                Resource3D resource = containerRequiredResources[i];
                spendSequence.Append(resource.transform.DOJump(resourceTarget.position, _spendJumpPower, 1, _spendDuration)
                    .OnComplete(() =>
                    {
                        _collector.RemoveResource(_amountPerTick, requirement.type);
                        AddToStorage(_amountPerTick, requirement.type);

                    }));

                yield return new WaitUntil(() => spendSequence.IsComplete());
                spendSequence.Kill();
                spendSequence = null;
                Destroy(containerRequiredResources[i].gameObject);
                _container3D.Remove(containerRequiredResources[i]);
            }
            containerRequiredResources.Clear();
        }
        StopSpending();
    }
    public override void AddToStorage(int amount, ResourceType type)
    {
        VisualRequireSetter visualRequireSetter = requireVisual[type].GetComponent<VisualRequireSetter>();
        visualRequireSetter.SetValue(visualRequireSetter.Value - amount);
        _fillImage.fillAmount += _fillOnePercent * amount;
        resourceStorage[type] += amount;
        if (haptic != null)
        {
            HapticController.Play(haptic);
        }

        Sequence addAnimation = DOTween.Sequence();

        addAnimation.Append(resourceTarget.transform.DOScale(startResourceTargetScale * 1.3f, 0.2f))
        .Append(resourceTarget.transform.DOScale(startResourceTargetScale, 0.2f))
        .OnComplete(() =>
        {
            if (CheckResourceCompletion())
            {
                StopSpending();
                ActivateEvent();
                return;
            }
        });
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
    private void ChangeFillColor()
    {
        _fillImage.color = _fillColor;
    }
    public override void ClearDictionaries()
    {
        resourceStorage.Clear();
        if (requireVisual.Count > 0)
        {
            foreach (GameObject visualRequire in requireVisual.Values)
            {
                Destroy(visualRequire);
            }
            requireVisual.Clear();
        }
    }
    public void LockSpend()
    {
        _isSpendingLocked = true;
    }
    public void UnlockSpend()
    {
        _isSpendingLocked = false;
    }
}
