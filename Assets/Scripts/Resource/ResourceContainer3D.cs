using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ResourceContainer3D : MonoBehaviour
{
    public List<GameObject> resourceModels = new List<GameObject>();
    public int resourceModelsAmount => resourceModels.Count;
    [SerializeField] private Transform _containerHolder;
    [SerializeField] float _resourceFlyDuration;
    [SerializeField] float _speedBoostPerTick;

    private Vector3 CalculateResourcePosition(GameObject resource) =>
        _containerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0);
    public void Add(GameObject resource)
    {
<<<<<<< Updated upstream:Assets/Scripts/ResourceContainer3D.cs

        resourceModels.Add(resource);
=======
        if (resources.IndexOf(resource) == 0)
        {
            return _containerHolder.position;
        }
        else
       {
            return  resources[resources.IndexOf(resource) - 1].transform.position + new Vector3(0, resource.LayoutObject.size.y/2f, 0);
        }
    }
    public void Add(Resource3D resource)
    {
        resources.Add(resource);
        _collector.AddResource(1, resource.Type);
>>>>>>> Stashed changes:Assets/Scripts/Resource/ResourceContainer3D.cs
        float duration = _resourceFlyDuration;
        var tweener = resource.transform
            .DOMove(CalculateResourcePosition(resource), duration)
            .SetEase(Ease.Linear)
        .SetAutoKill(false);
        tweener.OnPause(() => {
            tweener.Play();
            });
        tweener
            .OnUpdate(() =>
            {

                if (duration > 0.001f)
                {
                    duration -= _speedBoostPerTick;
                    if (duration < 0.001f)
                    {
                        duration = 0.001f;
                    }
                }
                if ((Vector3.Distance(resource.transform.position,
                        CalculateResourcePosition(resource)) > 0.15f))
                {
                    tweener.ChangeEndValue(CalculateResourcePosition(resource), duration, true);
                }

                else
                {
                    tweener.Kill();
                    resource.transform.parent = _containerHolder.transform;
                    resource.transform.position = CalculateResourcePosition(resource);
                }
 
               
            });
        
        resource.transform.DORotate(new Vector3(-90, 0, 0), 1f);
    }

    public void Remove(GameObject resource)
    {
        resourceModels.Remove(resource);
    }
}