using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceContainer3D : MonoBehaviour
{
    public List<GameObject> resourceModels = new List<GameObject>();
    public Transform ContainerHolder;
    public Vector3 TargetPosition;
    private Sequence _addSequence;
    [SerializeField] float _resourceFlyDuration;
    [SerializeField] float _resourceFollowDuration;

    [SerializeField] AnimationCurve _resourceFlyCurve; 
    public void AddResource(GameObject resource)
    {
        resourceModels.Add(resource);
        Tweener tweener = resource.transform.DOMove(ContainerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0), _resourceFlyDuration).OnComplete(() => resource.transform.parent = ContainerHolder.transform);
        tweener.SetUpdate(UpdateType.Fixed);
        tweener.OnUpdate(() =>
        {
            if (Vector3.Distance(resource.transform.position, (ContainerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource)))) > 0.2f)
            {
                Debug.Log(resourceModels.IndexOf(resource));
                tweener.ChangeEndValue(ContainerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0), _resourceFollowDuration, true);
            }
            else
            {
                resource.transform.parent = ContainerHolder.transform;
                resource.transform.position = ContainerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0);
                tweener.Kill();
                Debug.Log("Сработало");
            }
        });
            resource.transform.DORotate(new Vector3(-90, 0, 0), 1f);
        }
    private void FixedUpdate()
    {
        TargetPosition = ContainerHolder.position;
    }
    public void RemoveResource()
    {

    }
}
