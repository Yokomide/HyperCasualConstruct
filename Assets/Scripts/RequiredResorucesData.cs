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

    public ResourceRequirement[] requiredResources;
}