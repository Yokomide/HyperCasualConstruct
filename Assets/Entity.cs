using System;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected GameObject _hitFX;

    protected HealthComponent _healthComponent;
    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;
    protected Collider previousTarget;
    protected MobHolder _mobHolder;
    public Action<GameObject> OnDie;
    private void Start()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        _healthComponent.DecreaseHealth(amount);
        Instantiate(_hitFX, transform.position + Vector3.up, Quaternion.identity);
        if (_healthComponent.Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDie?.Invoke(gameObject);
        if (_mobHolder != null)
        {
            _mobHolder.Kill(this);
        }
        Destroy(gameObject);
    }

    public void InitializeHolder(MobHolder mobHolder)
    {
        _mobHolder = mobHolder;
    }
}
