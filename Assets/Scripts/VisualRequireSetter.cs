using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisualRequireSetter : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private List<ResourceObject> resourceObjectList = new List<ResourceObject>();

    public int Value { get; private set; }
    public void Initialize(ResourceType type, int value)
    {
        for (int i = 0; i < resourceObjectList.Count; i++)
        {
            if (resourceObjectList[i].Type == type)
            {
                SetIcon(resourceObjectList[i].Icon);
                SetValue(value);
            }
        }
    }
    private void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }
    public void SetValue(int value)
    {
        Debug.Log("Новое значение: " + value);
        Value = value;
        _value.text = value.ToString();
    }
}
