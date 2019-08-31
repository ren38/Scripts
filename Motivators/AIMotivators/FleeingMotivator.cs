using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingMotivator : BasicMotivator
{
    //navigation reference
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navAgent = null;

    [SerializeField]
    protected ObjectCombatable combatableSelf;
    protected GameObjectObserver hitWatch;

    void Start()
    {
        if(navAgent == null)
        {
            Debug.Log("Warning! no NavMeshAgent given " + this.gameObject.name);
        }
        if (combatableSelf == null)
        {
            Debug.Log("Warning! no object combatable script given " + this.gameObject.name);
        }
        hitWatch = gameObject.AddComponent<GameObjectObserver>();
        hitWatch.setupObserver(OnNext);
        combatableSelf.rawHitSubscribe(hitWatch);
    }


    public void OnNext(GameObject source)
    {
        Vector3 targetDir = source.transform.position - transform.position;
        Vector3 newDest = transform.position + -targetDir.normalized * 30;

        navAgent.SetDestination(newDest);
    }

    public void OnCompleted()
    {
        //unsub.Dispose();
    }
}
