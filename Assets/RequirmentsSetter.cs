using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirmentsSetter : MonoBehaviour
{
    [SerializeField] private Spender _spender;
    [SerializeField] private List<RequiredResourcesData> _requiredResources = new List<RequiredResourcesData>();

    public void SetRequirments(int index)
    {
        _spender.UpdateRequirments(_requiredResources[index]);    
    }
}
