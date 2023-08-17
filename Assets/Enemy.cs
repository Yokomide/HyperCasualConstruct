using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IDamageable
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform targetPoint; // ÷елева€ точка, к которой будет бежать враг
    [SerializeField] private float movementSpeed = 5f; // —корость перемещени€ врага
    [SerializeField] private float _attackRange = 1f; // –ассто€ние, на котором враг атакует других мобов
    [SerializeField] private int _damage;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float _searchRange;

    private IDamageable targetDamageReciever;
    private float nextAttackTime;
    private bool _isSearching = true;
    private bool _isAttack;
    private void Start()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(targetPoint.position);
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {

        if (_isSearching)
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
                _isSearching = false;
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(previousTarget.transform.position);
            }
        }

        if (!_isAttack)
            return;

        if (Time.time >= nextAttackTime)
        {
            if (previousTarget == null || targetDamageReciever == null)
            {
                _isAttack = false;
                _isSearching = true;
                _navMeshAgent.isStopped = false;

                _navMeshAgent.SetDestination(targetPoint.position);

                return;
            }
            if (Vector3.Distance(transform.position, previousTarget.transform.position) > _attackRange)
                return;

            _navMeshAgent.isStopped = true;
            _animator.SetTrigger("Attack");
            targetDamageReciever.TakeDamage(_damage);
            nextAttackTime = Time.time + attackCooldown;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, _attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, _searchRange);
    }

}
