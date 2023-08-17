using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
<<<<<<< Updated upstream
[RequireComponent(typeof(Collider))]
public class PickableResource3D : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private Collider _collider;
    [SerializeField] private int _amount;
=======
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PickableResource3D : MonoBehaviour
{
    [SerializeField] private GameObject _simpleResource;
    [SerializeField] private Collider _collider;
    private Resource3D _resource3D;
    private ICollector _collector;
    private ResourceContainer3D _container;
    public HapticClip haptic;
>>>>>>> Stashed changes

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ICollector collector))
            return;
        if (!other.TryGetComponent(out ResourceContainer3D container))
            return;
<<<<<<< Updated upstream
        collector.AddResource(_amount, _resourceType);
        container.Add(gameObject);
=======
        _collector = collector;
        _container = container;
>>>>>>> Stashed changes
        _collider.enabled = false;
        SpawnSimpleResource();
    }
    private void SpawnSimpleResource()
    {
        if (haptic != null)
        {
            if (HapticController.hapticsEnabled)
            {
                HapticController.Play(haptic);

            }
            else
            {
                HapticController.fallbackPreset = HapticPatterns.PresetType.Selection;
            }

        }
        var resource = Instantiate(_simpleResource);
        resource.transform.position = transform.position;
        _resource3D = resource.GetComponent<Resource3D>();
        Debug.Log("В начале: "+ _resource3D.LayoutObject);
        _container.Add(_resource3D);
        Destroy(gameObject);
    }
}
