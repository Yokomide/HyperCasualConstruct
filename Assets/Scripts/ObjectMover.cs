using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using UnityEngine.UIElements;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private MoveType _moveType;
    [SerializeField] private Transform _destination;

    [SerializeField] private float _moveDuration;
    [SerializeField] private float _speedBoostPerTick;

    [SerializeField] private bool _acceleration;
    [SerializeField] private bool shouldUpdateDestinationPosition;
    [SerializeField] private bool shouldChangeParent;
    public void Start()
    {
        MoveToDestination(_destination, _moveType, _acceleration, shouldUpdateDestinationPosition, shouldChangeParent);
    }
    public void MoveToDestination(Transform destination, MoveType moveType, bool acceleration, bool shouldUpdateDestinationPosition, bool shouldChangeParent)
    {
        switch(moveType)
        {
            case MoveType.Move:
                SimpleMove(destination, acceleration, shouldUpdateDestinationPosition, shouldChangeParent);
                break;

            case MoveType.Jump:

                break;
        }

    }
    private void SimpleMove(Transform destination, bool acceleration, bool shouldUpdateDestinationPosition, bool shouldChangeParent)
    {
        var duration = _moveDuration;
        var tweener = transform
            .DOMove(destination.position, duration);
        tweener
            .OnUpdate(() =>
            {
                if (acceleration)
                {
                    if (duration > 0.03f)
                    {
                        duration -= _speedBoostPerTick;
                        if (duration < 0.03f)
                        {
                            duration = 0.03f;
                        }
                    }
                }

                if (!shouldUpdateDestinationPosition)
                    return;

                tweener.ChangeEndValue(destination.position, duration, true);

                if (Vector3.Distance(transform.position, destination.position) > 0.05f)
                    return;

                if (shouldChangeParent)
                    transform.parent = destination.transform;

                tweener.Kill();
            });
    }
    private void JumpMove(Transform destination)
    {

    }
}
