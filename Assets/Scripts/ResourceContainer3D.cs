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
    private bool _isPlaying;
    private bool _isComplete;
    private bool _isActive;
    private bool _isUnityNull;
    private float _duration;
    private object _target;
    private Vector3 CalculateResourcePosition(GameObject resource) =>
        _containerHolder.position + new Vector3(0, 0.1f * resourceModels.IndexOf(resource), 0);
    public void Add(GameObject resource)
    {

        resourceModels.Add(resource);
        float duration = _resourceFlyDuration;
        var tweener = resource.transform
            .DOMove(CalculateResourcePosition(resource), duration)
            .SetEase(Ease.Linear)
        .SetAutoKill(false);
        SetPlayingBool(tweener);
        tweener.OnPause(() => {
            tweener.Play();
            Debug.Log("Пауза");
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
                Debug.Log(duration);
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
    private void SetPlayingBool(Tween tween)
    {
        _isPlaying = tween.IsPlaying();
        _isComplete = tween.IsComplete();
        _isActive = tween.IsActive();
        _isUnityNull = tween.IsUnityNull();
        _duration = tween.Duration();
        _target = tween.target;
    }
    public void Remove(GameObject resource)
    {
        resourceModels.Remove(resource);
    }
}
// tweener.SetUpdate(UpdateType.Fixed);
/*
tweener.OnComplete(() =>
{
    if ((Vector3.Distance(resource.transform.position,
                CalculateResourcePosition(resource)) > 0.06f))
    {
        if (duration > 0.01f)
        {
            duration -= _speedBoostPerTick;
            if (duration < 0.01f)
            {
                duration = 0.01f;
            }
        }
        tweener.ChangeEndValue(CalculateResourcePosition(resource), duration, true);
        tweener.Play();
    }
    else
    {
        resource.transform.parent = _containerHolder.transform;
        resource.transform.position = CalculateResourcePosition(resource);
        tweener.Kill();
    }




}
);
*/
//tweener.SetUpdate(UpdateType.Fixed);