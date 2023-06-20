using UnityEngine;

internal interface ICollector
{
    public int GetResourceAmount(ResourceType resourceType);
    public void AddResource(int amount, ResourceType resource);
    public void RemoveResource(int amount, ResourceType resource);
    public void AddItem(int amount, string type);

    public void PlayOneShotCollectAnimation();

    public void StartAnimation(CharacterAnimationType characterAnimationType);
    public void EndAnimation();


}