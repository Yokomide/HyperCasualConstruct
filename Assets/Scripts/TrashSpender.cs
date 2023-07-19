using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpender : MonoBehaviour
{
    [SerializeField] protected float _spendDelay;
    [SerializeField] protected float _spendSpeed;

    private ICollector _collector;
    private ResourceContainer3D _container3D;
    private Coroutine spending3DCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        _collector = collector;

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
    private void StartSpending()
    {
        spending3DCoroutine = StartCoroutine(Spend());
    }
    private void StopSpending()
    {

    }
    IEnumerator Spend()
    {
        if (_container3D.resourceAmount <= 0)
        {
            StopSpending();
            yield break;
        }
        foreach(var resource in _container3D.resources)
        {/*
                var targetStartScale = resourceTarget.localScale;
                var spendSequence = DOTween.Sequence();
                spendSequence.SetAutoKill(false);
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
            */
            }
        StopSpending();
           
    }
}
