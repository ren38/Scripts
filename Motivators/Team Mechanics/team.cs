using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class team : MonoBehaviour
{
    [SerializeField]
    protected List<ObjectActor> actorObjects;
    //private league myLeague;
    protected List<team> enemyTeams;
    protected List<ObjectCombatable> enemyIndividuals;

    public virtual List<ObjectActor> getActorObjects() { return actorObjects; }

    protected void setup()
    {
        enemyTeams = new List<team>();
        enemyIndividuals = new List<ObjectCombatable>();
        foreach (ObjectActor subject in actorObjects)
        {
            subject.setTeam(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void addEnemyTeam(team newTeam)
    {
        if (!enemyTeams.Contains(newTeam))
        {
            enemyTeams.Add(newTeam);
        }
    }

    public void removeEnemyTeam(team oldTeam)
    {
        if(enemyTeams.Contains(oldTeam))
        {
            enemyTeams.Remove(oldTeam);
        }
    }

    public virtual void addEnemyIndividual(ObjectCombatable newIndividual)
    {
        if (!enemyIndividuals.Contains(newIndividual))
        {
            enemyIndividuals.Add(newIndividual);
        }
    }

    public void removeEnemyIndividual(ObjectCombatable newIndividual)
    {
        if (enemyIndividuals.Contains(newIndividual))
        {
            enemyIndividuals.Remove(newIndividual);
        }
    }

    private bool combat = false;
}