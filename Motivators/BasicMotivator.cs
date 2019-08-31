using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicMotivator : MonoBehaviour
{
    [SerializeField]
    protected ObjectActor actorSelf;
    protected team myTeam;
    protected bool inCombat = false;
    protected List<ObjectActor> targets;

    public void checkTargetList()
    {
        if(targets == null)
        {
            targets = new List<ObjectActor>();
        }
    }

    public virtual void newTargetIndividual(ObjectActor newTarget)
    {
        checkTargetList();
        targets.Add(newTarget);
        inCombat = true;
    }
    
    public virtual void newTargetGroup(List<ObjectActor> newList)
    {
        checkTargetList();
        foreach (ObjectActor actor in newList)
        {
            targets.Add(actor);
        }
        inCombat = true;
    }

    public void leaveCombat()
    {
        inCombat = false;
        targets.Clear();
    }

    public void interestLost()
    {
        inCombat = false;
        actorSelf.abruptSkillCancel();
        actorSelf.clearQueue();
    }

    public virtual void dying() { interestLost(); }
    public void joinTeam(team i) { myTeam = i; }
}
