using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{
    private Transform _destination;
    public Transform Destination
    {
        get { return _destination; }
        set
        {

            _destinationVector3 = value.position;
            _destination = value;

        }
    }

    public Vector3 _destinationVector3;

    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _animWork;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    public void Move()
    {
        _agent.SetDestination(_destinationVector3);
        _animator.SetBool("isWalking", true);
        _animWork = true;

    }
    private void FixedUpdate()
    {

        if (_animWork == true)
        {
          //  Debug.Log(Vector3.Distance(_agent.transform.position, _destinationVector3));
            if (Vector3.Distance(_agent.transform.position, _destinationVector3) <= 1)
            {
                _animator.SetBool("isWalking", false);
                _animWork = false;
            }
        }

    }
}
