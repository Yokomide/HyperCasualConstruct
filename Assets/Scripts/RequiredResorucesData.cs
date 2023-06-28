using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Building Data", menuName = "Building Data")]
public class RequiredResourcesData : ScriptableObject
{
    [System.Serializable]
    public class ResourceRequirement
    {
        public ResourceType type;
        public int amount;
    }
    public List<ResourceRequirement> requiredResources = new List<ResourceRequirement>();
}