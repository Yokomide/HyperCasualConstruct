using DG.Tweening;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;
using Vector3 = UnityEngine.Vector3;

public class ResourceSpender : MonoBehaviour, IEventActivator
{
    [Header("====Resource Settings====")]
    [SerializeField] public RequiredResourcesData _requiredResources;


    [Space(10)]

    [Header("====Spend Settings====")]
    [SerializeField] private float _spendDelay;
    [SerializeField] private float _spendSpeed;
    [SerializeField] private float _spendCancelSpeed;
    [SerializeField] private float _spendJumpPower;
    [SerializeField] private float _spendDuration;
    [Space(5)]
    [SerializeField] private bool _isSpendingLocked;
    [SerializeField] private int _amountPerTick;
    [SerializeField] private bool _suckResource3D;
    [Space(10)]

    [Header("====Animation====")]
    [SerializeField] private bool SpendAnimation;
    [SerializeField] private CharacterAnimationType _characterAnimationType;
    [SerializeField] private float _animationSpeed;
    [Space(10)]

    [Header("====Visual====")]
    [OnValueChanged("ChangeFillColor")]
    [SerializeField] private Color _fillColor;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Transform _UIHolder;
    [SerializeField] private GameObject _UIPrefab;
    [SerializeField] public Transform resourceTarget;

    [HideInInspector] public event Action OnEventActivate;

    [Header("====Feelings====")]
    public HapticClip haptic;


    [HideInInspector] public Vector3 startResourceTargetScale;

    private Dictionary<ResourceType, int> resourceStorage = new Dictionary<ResourceType, int>();

    private Dictionary<ResourceType, GameObject> requireVisual = new Dictionary<ResourceType, GameObject>();

    private List<Resource3D> containerRequiredResources = new List<Resource3D>();

    private ICollector _collector;
    private ResourceContainer3D _container3D;

    private float _fillMaxAmount;
    private float _fillOnePercent;

    private bool _isSpendingActive;

    private Coroutine spending3DCoroutine;
    private void OnTriggerEnter(Collider other)
    {
        if (_isSpendingLocked)
            return;
        if (_requiredResources == null)
            return;
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _collector = collector;
        _isSpendingActive = true;
        if (SpendAnimation)
            collector.StartAnimation(_characterAnimationType, _animationSpeed);
        if (_suckResource3D)
        {
            _container3D = other.GetComponent<ResourceContainer3D>();
            spending3DCoroutine = StartCoroutine(StartSpending3D());
            return;
        }
        StartCoroutine(StartSpendingWithDelay());
    }

    private void OnTriggerExit(Collider other)
    {
        if (_collector == null)
        {
            return;
        }
        if (other.TryGetComponent(out ICollector collector))
            return;
        StopSpending();
    }
    public void UpdateRequirments(RequiredResourcesData requiredResourcesData)
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
                Debug.Log("Прошло ожидание");
            }
            Debug.Log("Вышел из цикла");
            containerRequiredResources.Clear();
        }
    }


    private void StartSpending()
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
    private bool CheckResourceCompletion()
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

    private void AddToStorage(int amount, ResourceType type)
    {
        VisualRequireSetter visualRequireSetter = requireVisual[type].GetComponent<VisualRequireSetter>();
        visualRequireSetter.SetValue(visualRequireSetter.Value - amount);
        _fillImage.fillAmount += _fillOnePercent * amount;
        resourceStorage[type] += amount;
        if (haptic != null)
        {
            HapticController.fallbackPreset = HapticPatterns.PresetType.Selection;
            HapticController.Play(haptic);
        }
        if (_suckResource3D)
        {
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
        else
        {
            if (CheckResourceCompletion())
            {
                StopSpending();
                _requiredResources = null;
                ActivateEvent();
                return;
            }
        }
    }
    private void StopSpending()
    {
        _isSpendingActive = false;
        if (SpendAnimation)
            _collector.EndAnimation();
        containerRequiredResources.Clear();
        if (spending3DCoroutine != null)
        {
            StopCoroutine(spending3DCoroutine);
            spending3DCoroutine = null;
        }
        _collector = null;
    }
    private void ChangeFillColor()
    {
        _fillImage.color = _fillColor;
    }
    public void ClearDictionaries()
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
    public void ActivateEvent()
    {
        _fillImage.DOFillAmount(0, 0.1f);
        OnEventActivate?.Invoke();
    }
    IEnumerator StartSpendingWithDelay()
    {
        while (_isSpendingActive && !_isSpendingLocked)
        {
            StartSpending();
            yield return new WaitForSeconds(_spendDelay);
        }
        yield return null;
    }
}
