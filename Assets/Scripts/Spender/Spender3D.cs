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

    private List<Resource3D> containerRequiredResources = new List<Resource3D>();

    private ResourceContainer3D _container3D;
    public ResourceContainer3D Containter3D => _container3D;

    private Coroutine spending3DCoroutine;
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


    public override void StartSpending()
    {
        spending3DCoroutine = StartCoroutine(Spend());
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

    IEnumerator Spend()
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

        Sequence addAnimation = DOTween.Sequence();

        addAnimation.Append(resourceTarget.transform.DOScale(startResourceTargetScale * 1.3f, 0.2f))
        .Append(resourceTarget.transform.DOScale(startResourceTargetScale, 0.2f))
        .OnComplete(() =>
        {
            if (CheckResourceCompletion())
            {
                StopSpending();
                ActivateEvent();
                if (_initializeRequrimentsOnStart)
                    UpdateRequirments(_requiredResources);
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
}
