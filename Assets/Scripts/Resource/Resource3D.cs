using AutoLayout3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource3D : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    public ResourceType Type { get { return _resourceType; } }

    private bool _isUpgradeable;
    public bool IsUpgradeable { get { return _isUpgradeable; } }


    private GameObject _gameObject;
    public GameObject GameObject { get { return _gameObject; } }


    private LayoutElement3D _layoutObject;
    public LayoutElement3D LayoutObject { get { return _layoutObject; } }

    
    private void Awake()
    {
        _gameObject = GetComponent<GameObject>();
        _layoutObject = GetComponent<LayoutElement3D>();
    }
}
