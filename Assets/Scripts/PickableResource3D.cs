using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class PickableResource3D : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private Collider _collider;
    [SerializeField] private int _amount;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        if (!other.TryGetComponent(out ResourceContainer3D container))
            return;
        collector.AddResource(_amount, _resourceType);
        container.Add(gameObject);
        _collider.enabled = false;
       
    }
}
