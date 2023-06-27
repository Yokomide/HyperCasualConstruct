using UnityEngine;

[CreateAssetMenu(fileName = "New Spender Data", menuName = "Spender Storage Data")]
public class SpenderStorageData : ScriptableObject
{
    [System.Serializable]
    public class ResourceStorage
    {
        public ResourceType type;
        public int amount;
    }

    public ResourceStorage[] resourceStorage;
}