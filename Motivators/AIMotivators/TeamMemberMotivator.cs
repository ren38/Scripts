using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMemberMotivator : BasicMotivator
{
    private GameObject[] skillBar;
    private System.Random rand;
    private int skillCount;
    [SerializeField]
    private int planning = 1;
    private float timer;
    private ObjectActor targetActor;

    void Start()
    {
        skillBar = actorSelf.getSkillBar();
        rand = new System.Random();
        skillCount = skillBar.Length;
    }

    public override void newTargetIndividual(ObjectActor newTarget)
    {
        base.newTargetIndividual(newTarget);
        timer = Time.time + 20.0f;
    }

    public override void newTargetGroup(List<ObjectActor> newList)
    {
        base.newTargetGroup(newList);
        timer = Time.time + 20.0f;
    }

    private void changeTarget()
    {
        targetActor = targets[rand.Next(targets.Count - 1)];
    }

    void Update()
    {
        // loses interest
        if (inCombat && (Time.time >= timer))
        {
            interestLost();
        }


        if (inCombat)
        {
            if (targetActor == null || targetActor.getDeathState() || rand.Next(1000) == 0)
            {
                changeTarget();
            }
            if (actorSelf.queueCount() < planning)
            {
                actorSelf.skillEnqueue(rand.Next(skillCount - 1), targetActor);
            }
        }
    }
}
