using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class UIValueUpdater : MonoBehaviour, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
    [SerializeField, HideInInspector]
    private SerializationData serializationData;

    SerializationData ISupportsPrefabSerialization.SerializationData { get { return this.serializationData; } set { this.serializationData = value; } }

    //[SerializeField] private IStorage _storage;
    [SerializeField] private ResourceStorage _resourceStorage;

    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        // _storage.OnValueChanged += SetValue;
        _resourceStorage.OnValueChanged += SetValue;
    }

    private void OnDisable()
    {
        // _storage.OnValueChanged += SetValue;
        _resourceStorage.OnValueChanged -= SetValue;
    }

    private void Start()
    {
        SetValue();
    }

    private void SetValue()
    {
        switch (_resourceType)
        {
            case ResourceType.Gold:
                _text.text = _resourceStorage.GoldAmount.ToString();
                break;

            case ResourceType.BlueDiamond:
                _text.text = _resourceStorage.BlueDiamondAmount.ToString();
                break;

            case ResourceType.RedDiamond:
                _text.text = _resourceStorage.RedDiamondAmount.ToString();
                break;

            case ResourceType.Wood:
                _text.text = _resourceStorage.WoodAmount.ToString();
                break;
        }
    }

    public void OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
    }

    public void OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
    }
}