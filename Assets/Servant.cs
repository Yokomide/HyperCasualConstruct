using System;
using UnityEngine;
using UnityEngine.AI;

public class Servant : Entity, IDamageable
{

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private int _damage;

    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _searchRange;
    private float nextAttackTime;
    [SerializeField] private float attackCooldown;

    private bool _isSearchingEnemy = true;
    private bool _isAttack;
    private IDamageable targetDamageReciever;

    private void Update()
    {
        if (_isSearchingEnemy)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up, _searchRange, _layerMask);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IDamageable target))
                {
                    if (previousTarget == null)
                    {
                        previousTarget = collider;
                        targetDamageReciever = target;
                        continue;
                    }
                    if (Vector3.Distance(transform.position, collider.transform.position) < Vector3.Distance(transform.position, previousTarget.transform.position))
                    {
                        previousTarget = collider;
                        targetDamageReciever = target;

                    }
                    else
                        continue;
                }
            }
            if (previousTarget != null && targetDamageReciever != null)
            {
                _isAttack = true;
                _isSearchingEnemy = false;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(previousTarget.transform.position);
            }
        }

        if (!_isAttack)
            return;

        if (Time.time >= nextAttackTime)
        {
            Attack();
        }
    }
    public void Attack()
    {
        if (previousTarget == null || targetDamageReciever == null)
        {
            _isAttack = false;
            _isSearchingEnemy = true;
            return;
        }
        if (Vector3.Distance(transform.position, previousTarget.transform.position) > _attackRange)
            return;

        _navMeshAgent.isStopped = true;
        _animator.SetTrigger("Attack");
        Debug.Log("Нанёс урон: " + _damage);
        targetDamageReciever.TakeDamage(_damage);
        nextAttackTime = Time.time + attackCooldown;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, _attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, _searchRange);

    }
}
