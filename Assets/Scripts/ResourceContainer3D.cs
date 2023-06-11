using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

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
