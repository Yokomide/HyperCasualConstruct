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
    public void StartCollectAnimation()
    {
        _animator.SetBool("isFarming", true);
    }

    public void EndCollectAnimation()
    {
        _animator.SetBool("isFarming", false);
    }

    public void PlayOneShotCollectAnimation()
    {
        // _animator.Play("");
    }

    public void StartSpendAnimation()
    {
        _animator.SetBool("isSpending", true);

    }

    public void EndSpendAnimation()
    {
        _animator.SetBool("isSpending", false);


    }
}