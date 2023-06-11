using UnityEngine;

internal interface ICollector
{
    public void AddResource(int amount, ResourceType resource);

    public void AddItem(int amount, string type);

    public void PlayOneShotCollectAnimation();

    public void StartCollectAnimation();

    public void EndCollectAnimation();
}