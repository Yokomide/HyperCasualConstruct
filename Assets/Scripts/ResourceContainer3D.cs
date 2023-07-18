using AutoLayout3D;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ResourceContainer3D : MonoBehaviour
{
    public List<Resource3D> resources = new List<Resource3D>();
    public int resourceAmount => resources.Count;
    [SerializeField] private Transform _containerHolder;
    [SerializeField] float _resourceFlyDuration;
    [SerializeField] float _speedBoostPerTick;
    [SerializeField] YAxisLayoutGroup3D layout;
    private Vector3 CalculateResourcePosition(Resource3D resource)
    {
        if (resources.IndexOf(resource) == 0)
        {
            return _containerHolder.position;
        }
        else
       {
            return  resources[resources.IndexOf(resource) - 1].transform.position + new Vector3(0, resource.LayoutObject.size.y, 0);
        }
    }
    public void Add(Resource3D resource)
    {
        Debug.Log("Вот ресурс: " + resource);
        resources.Add(resource);
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
        
    }

    public void Remove(Resource3D resource)
    {
        resources.Remove(resource);
    }
}