using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenedEffect : MonoBehaviour, IEffect
{
    private const float WEAKENEDDURATION = 30.0f;
    private const float ATTACKLOSS = 0.3f;
    private int multiple = 1;
    private const int MAXMULTIPLIER = 3;
    protected ObjectActor subject;
    protected ObjectInteractable source;

    protected int conditionID;
    protected List<GameObject> instanceList;
    protected string effectName;
    protected string description;
    protected bool timed;
    protected float duration;
    protected float endTime;

    public void setup(ObjectActor subject, ObjectInteractable source)
    {
        instanceList = new List<GameObject>();
        timed = true;
        duration = WEAKENEDDURATION;
        endTime = duration + Time.time;
        effectName = "Weakened";
        description = string.Format("Basic attack power reduced by {0}%.", ATTACKLOSS * 100);
        this.subject = subject;
        this.source = source;
        subject.attackModChange(-ATTACKLOSS);
    }

    public void apply(float deltaTime)
    {
        return;
    }

    public float getEnd()
    { return endTime; }

    public void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public void end(ObjectActor subject)
    {
        subject.attackModChange(ATTACKLOSS * multiple);
        Destroy(this);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
    }


    public int getMult()
    {
        return multiple;
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(6));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, name, description, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
            subject.attackModChange(-ATTACKLOSS);
        }
        string newName = ("Weakened x" + multiple);
        string newDescription = string.Format("Basic attack power reduced by {0}%.", ATTACKLOSS * multiple * 100);
        bool timed = true;
        effectFunctions.iconUpdate(instanceList, newName, newDescription, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }
}

