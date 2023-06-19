using System.Collections.ObjectModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ResourceSpender : MonoBehaviour
{
    [Header("====Resource Settings====")]
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private ResourceStorage _resourceStorage;
    [Space(10)]

    [Header("====Farm Settings====")]
    [SerializeField] private float _spendDelay;
    [SerializeField] private float _spendSpeed;
    [SerializeField] private float _spendCancelSpeed;
    [SerializeField] private float _spendJumpPower;
    [SerializeField] private float _spendDuration;

    [SerializeField] private int _amountPerTick;
    [SerializeField] private bool _suckResource3D;
    [Space(10)]

    [Header("====Animation====")]
    [SerializeField] private bool SpendAnimation;
    [Space(10)]

    [Header("====Visual====")]
    [OnValueChanged("ChangeFillColor")]
    [SerializeField] private Color _fillColor;
    [SerializeField] private Image _fillImage;

    private ResourceContainer3D _container3D;
    private Sequence _spendSequence;


    private bool _isSpendingActive;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _isSpendingActive = true;
        if(_suckResource3D)
            _container3D = other.GetComponent<ResourceContainer3D>();
        StartSpending(collector, other.gameObject);

        if (SpendAnimation)
            collector.StartSpendAnimation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        StopSpending(collector);
    }
    private void StartSpending(ICollector collector, GameObject interactor)
    {
        if(_isSpendingActive)
        {
            _spendSequence = DOTween.Sequence();
            if (_suckResource3D)
            {

                if (_container3D.resourceModels.Count <= 0)
                    StopSpending(collector);

                var resourceModel = _container3D.resourceModels[_container3D.resourceModelsAmount - 1];
                _spendSequence.Append(_container3D.resourceModels[_container3D.resourceModelsAmount - 1].transform.DOJump(transform.position, _spendJumpPower, 1, _spendDuration )
                    .OnComplete(() =>
            {
                _container3D.Remove(_container3D.resourceModels[_container3D.resourceModelsAmount - 1]);
                Destroy(resourceModel);
                StartSpending(collector, interactor);
                collector.RemoveResource(_amountPerTick, _resourceType);
            }));
                
            }
        }
    }
    private void StopSpending(ICollector collector)
    {
        _isSpendingActive = false;
        if (SpendAnimation)
            collector.EndSpendAnimation();
    }
    private void ChangeFillColor()
    {
        _fillImage.color = _fillColor;
    }
}
