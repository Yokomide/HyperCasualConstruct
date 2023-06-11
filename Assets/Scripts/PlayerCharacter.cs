using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PlayerMovement))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AddItem(int amount, string type)
    {
        Debug.Log("Добавлено: " + type + " в количестве " + amount);
    }

    public void AddResource(int amount, string type)
    {
        Debug.Log("Добавлено: " + type + " в количестве " + amount);
    }

    public void StartCollectAnimation()
    {
        _animator.SetBool("isDancing", true);
    }

    public void EndCollectAnimation()
    {
        _animator.SetBool("isDancing", false);
    }

    public void PlayOneShotCollectAnimation()
    {
        _animator.Play("");
    }
}