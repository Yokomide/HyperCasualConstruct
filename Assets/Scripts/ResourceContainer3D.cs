using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
/*
public class ResourceContainer3D : MonoBehaviour
{
    public List<GameObject> resourceModels = new List<GameObject>();

    [SerializeField] private Transform _containerHolder;
    [SerializeField] private AnimationCurve _resourceFlyCurve;

    [SerializeField] float _resourceFlyDuration;

    public void AddResource(GameObject resource)
    {
        resourceModels.Add(resource);

        Tweener tweener = resource.transform.DOMove(_containerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0), _resourceFlyDuration).OnComplete(() => resource.transform.parent = _containerHolder.transform);
        tweener.SetUpdate(UpdateType.Fixed);
        tweener.OnUpdate(() =>
        {
            if (Vector3.Distance(resource.transform.position, (_containerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource)))) > 0.2f)
            {
                Debug.Log(resourceModels.IndexOf(resource));
                tweener.ChangeEndValue(_containerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0), true);
            }
            else
            {
                resource.transform.parent = _containerHolder.transform;
                resource.transform.position = _containerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0);
                tweener.Kill();
                Debug.Log("Сработало");
            }
        });
            resource.transform.DORotate(new Vector3(-90, 0, 0), 1f);
        }
    public void RemoveResource()
    {

    }
}
*/
public class ResourceContainer3D : MonoBehaviour
{
    public List<GameObject> resourceModels = new List<GameObject>();
    [SerializeField] private Transform _containerHolder;
    [SerializeField] float _resourceFlyDuration;
    [SerializeField] float _speedBoostPerTick;

    private Vector3 CalculateResourcePosition(GameObject resource) =>
        _containerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0);

    public void Add(GameObject resource)
    {
        resourceModels.Add(resource);
        float duration = _resourceFlyDuration;
        var position = CalculateResourcePosition(resource);
        var tweener = resource.transform
            .DOMove(position, duration)
            .OnComplete(() => resource.transform.parent = _containerHolder.transform)
            .SetUpdate(UpdateType.Late);
        tweener
            .OnUpdate(() =>
            {
                if (duration > 0.03f)
                {
                    duration -= _speedBoostPerTick;
                    if (duration < 0.03f)
                    {
                        duration = 0.03f;
                    }
                }

                Debug.Log(duration);
                tweener.ChangeEndValue(CalculateResourcePosition(resource), duration, true);
                if ((Vector3.Distance(resource.transform.position,
                        CalculateResourcePosition(resource)) > 0.1f)) return;
                resource.transform.parent = _containerHolder.transform;
                resource.transform.position = CalculateResourcePosition(resource);
                tweener.Kill();
            });
        resource.transform.DORotate(new Vector3(-90, 0, 0), 1f);
    }

    public void Remove()
    {
    }
}