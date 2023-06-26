using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Player Resources", menuName = "Player/Resources/Resource")]
public class ResourceObject : ScriptableObject//, IStorage
{
    [SerializeField] public Sprite Icon;
    [SerializeField] public GameObject Object;
    [SerializeField] public ResourceType Type;

    
}