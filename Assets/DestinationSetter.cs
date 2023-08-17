using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationSetter : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private ObjectSpawner _spawner;
    [SerializeField] private MobHolder _mobHolder;
    private void OnEnable()
    {
        _spawner.OnObjectsSpawned += StartAddToArmy;
    }
    private void OnDisable()
    {
        _spawner.OnObjectsSpawned -= StartAddToArmy;
    }
    /*
    public void SetDestination(GameObject gameObject)
    {
        var agent = gameObject.GetComponent<SimpleAI>();
        agent.Destination = _destination;
        StartCoroutine(Waiter(agent));
    }
    */
    private void StartAddToArmy(GameObject mob)
    {
        StartCoroutine(AddToArmy(mob));
    }
    IEnumerator AddToArmy(GameObject mob)
    {
        yield return new WaitForSeconds(1f);
        if (mob.TryGetComponent(out Entity entity))
        {
            _mobHolder.AddMob(entity);
            entity.InitializeHolder(_mobHolder);
        }

    }
}
