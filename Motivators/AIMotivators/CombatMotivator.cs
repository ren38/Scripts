using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMotivator : BasicMotivator
{
    protected GameObjectObserver hitWatch;
    //protected Unsubscriber<GameObject> unsub;
    private ObjectActor targetActor;
    [SerializeField]
    private float timer;
    private GameObject[] skillBar;
    private int skillCount;
    private System.Random rand;
    [SerializeField]
    private int planning = 1;

    void Start()
    {
        /*
        if (navAgent == null)
        {
            Debug.Log("Warning! no NavMeshAgent given " + this.gameObject.name);
        }
        */
        if (actorSelf == null)
        {
            Debug.Log("Warning! no object actor script given " + this.gameObject.name);
        }
        hitWatch = gameObject.AddComponent<GameObjectObserver>();
        hitWatch.setupObserver(OnNext);
        actorSelf.rawHitSubscribe(hitWatch);
        skillBar = actorSelf.getSkillBar();
        rand = new System.Random();
        skillCount = skillBar.Length;
    }

    void Update()
    {
        // loses interest
        if (inCombat && (targetActor.getDeathState() || Time.time >= timer))
        {
            interestLost();
        }
        
        if(inCombat && (rand.Next(1000) == 0 || targetActor.getDeathState() || targetActor == null))
        {
            // this should use an observable to trigger getDeathState. only uses n cycles during combat.
            // where n = number of combat motivators involved.
            getRandomTarget();
        }

        if(inCombat && actorSelf.queueCount() < planning)
        {
            actorSelf.skillEnqueue(rand.Next(skillCount - 1), targetActor);
        }
    }

    public void OnNext(GameObject source)
    {
        if(!inCombat)
        {
            targetActor = source.GetComponent<ObjectActor>();
            inCombat = true;
            timer = Time.time + 20.0f;
        }
    }


    public void OnCompleted()
    {
        //unsub.Dispose();
    }

    private ObjectActor getRandomTarget()
    {
        return targets[rand.Next(targets.Count - 1)];
    }

    public override void dying()
    {
        base.dying();
    }
}
