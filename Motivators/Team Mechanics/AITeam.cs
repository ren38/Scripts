using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITeam : team
{
    [SerializeField]
    protected List<BasicMotivator> motivatorUnits;
    private bool teamDead = false;

    public List<BasicMotivator> getMotivatorUnits() { return motivatorUnits; }

    // Start is called before the first frame update
    void Start()
    {
        teamSetup();
    }

    public void teamSetup()
    {
        base.setup();
        foreach (BasicMotivator unit in motivatorUnits)
        {
            unit.joinTeam(this);
        }
        foreach (ObjectActor actor in actorObjects)
        {
            comboObserver obs = gameObject.AddComponent<comboObserver>();
            obs.setupObserver(trigger);
            actor.targettedSubscribe(obs);
            GameObjectObserver deadMember = gameObject.AddComponent<GameObjectObserver>();
            deadMember.setupObserver(removeDead);
            actor.deathSubscribe(deadMember);
        }
    }

    protected virtual void trigger((ObjectInteractable, BaseSkill) data)
    {
        if (determineIfHostile(data.Item2))
        {
            ObjectActor newHostile = data.Item1 as ObjectActor;
            if (newHostile != null)
            {
                team targetTeam = newHostile.getTeam();
                if(targetTeam != null)
                {
                    if(targetTeam == this)
                    {
                        return;
                    }
                    notifyAllTeam(targetTeam);
                }
                else
                {
                    notifyAllIndividual(newHostile);
                }
            }
        }
    }

    protected virtual bool determineIfHostile(BaseSkill skill)
    {
        List<skillType> types = skill.getTypes();
        foreach(skillType type in types)
        {
            if (type == skillType.attack || type == skillType.curse)
                return true;
        }
        return false;
    }

    private void notifyAllIndividual(ObjectActor newTarget)
    {
        foreach (BasicMotivator unit in motivatorUnits)
        {
            unit.newTargetIndividual(newTarget);
        }
    }


    private void notifyAllTeam(team newHostileTeam)
    {
        foreach (BasicMotivator unit in motivatorUnits)
        {
            unit.newTargetGroup(newHostileTeam.getActorObjects());
        }
    }

    private void removeDead(GameObject newDead)
    {
        foreach (ObjectActor actor in actorObjects)
        {
            if (actor.getDeathState())
            {
                BasicMotivator deadActor = actor.GetComponent<BasicMotivator>();
                if (deadActor != null)
                {
                    motivatorUnits.Remove(deadActor);
                }
            }
        }
        if(motivatorUnits.Count == 0)
        {
            teamDead = true;
        }
    }
}
