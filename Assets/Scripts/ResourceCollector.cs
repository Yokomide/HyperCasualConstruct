using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour, ICollector
{
    [SerializeField] private ResourceStorage _resourceStorage;

    private Animator _animator;

    private void Start()
    {
        if (TryGetComponent(out Animator anim))
            _animator = anim;
    }

    public void AddItem(int amount, string type)
    {
    }

    public void AddResource(int amount, ResourceType resource)
    {
        _resourceStorage.AddValue(amount, resource);
    }
    public void RemoveResource(int amount, ResourceType resource)
    {
        _resourceStorage.RemoveValue(amount, resource);
    }
    public void StartAnimation(CharacterAnimationType characterAnimationType)
    {
        switch(characterAnimationType)
        {
            case CharacterAnimationType.Dance_1:
                _animator.SetBool("isDancing_1", true);
                break;
            case CharacterAnimationType.Dance_2:
                _animator.SetBool("isDancing_2", true);
                break;
            case CharacterAnimationType.Mine:
                _animator.SetBool("isMining", true);
                break;
        }
    }

    public void EndAnimation()
    {
        _animator.SetBool("isDancing_1", false);
        _animator.SetBool("isDancing_2", false);
        _animator.SetBool("isMining", false);

    }

    public void PlayOneShotCollectAnimation()
    {
        // _animator.Play("");
    }


    public int GetResourceAmount(ResourceType resourceType)
    {
        return _resourceStorage.GetAmount(resourceType);
    }
}