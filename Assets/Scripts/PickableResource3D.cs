using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider),typeof(Resource3D))]

public class PickableResource3D : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private int _amount;
    private Resource3D _resource3D;

    private void Start()
    {
        _resource3D = GetComponent<Resource3D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        if (!other.TryGetComponent(out ResourceContainer3D container))
            return;
        collector.AddResource(_amount, _resource3D.Type);
        container.Add(_resource3D);
        _collider.enabled = false;
       
    }
}
