using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixTeam : AITeam
{
    [SerializeField]
    private PlayerMotivator player;
    [SerializeField]
    private ObjectActor Leader;
    [SerializeField]
    private Transform[] positionsAroundLeader;

    // Start is called before the first frame update
    void Start()
    {
        teamSetup();
        player.setTeam(this);
    }

    private void Update()
    {
        givePositions();
    }

    private void givePositions()
    {
        if(enemyIndividuals == null || enemyIndividuals.Count == 0)
        {
            int i = 0;
            foreach (BasicMotivator member in motivatorUnits)
            {
                member.setNewDestination(positionsAroundLeader[i].position);
            }
        }
    }

    public override void addEnemyTeam(team newTeam)
    {
        if (newTeam != this)
        {
            base.addEnemyTeam(newTeam);
            foreach (BasicMotivator unit in motivatorUnits)
            {
                unit.newTargetGroup(newTeam.getActorObjects());
            }
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