using TMPro;
using UnityEngine;

public class UIValueUpdater : MonoBehaviour
{
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private ResourceStorage _resourceStorage;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _resourceStorage.OnValueChanged += SetValue;
    }

    private void OnDisable()
    {
        _resourceStorage.OnValueChanged -= SetValue;
    }

    private void Start()
    {
        SetValue();
    }

    private void SetValue()
    {
        _text.text = _resourceStorage.GetAmount(_resourceType).ToString();
    }
}