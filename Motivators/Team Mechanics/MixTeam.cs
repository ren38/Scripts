using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixTeam : AITeam
{
    [SerializeField]
    private PlayerMotivator player;
    [SerializeField]
    private ObjectActor Leader;

    // Start is called before the first frame update
    void Start()
    {
        teamSetup();
        player.setTeam(this);
    }

    public override void addEnemyTeam(team newTeam)
    {
        base.addEnemyTeam(newTeam);
        foreach (BasicMotivator unit in motivatorUnits)
        {
            unit.newTargetGroup(newTeam.getActorObjects());
        }
    }

    public override void addEnemyIndividual(ObjectCombatable newIndividual)
    {
        base.addEnemyIndividual(newIndividual);
        ObjectActor actor = (ObjectActor) newIndividual;
        if (actor != null)
        {
            foreach (BasicMotivator unit in motivatorUnits)
            {
                unit.newTargetIndividual(actor);
            }
        }
    }
}