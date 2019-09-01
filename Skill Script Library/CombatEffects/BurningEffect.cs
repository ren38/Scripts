using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : MonoBehaviour, IEffect
{
    private const float BURNINGDURATION = 3.0f;
    private const float DEGENSPEED = 7.0f;
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
        duration = BURNINGDURATION;
        endTime = duration + Time.time;
        effectName = "Burning";
        description = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        this.subject = subject;
        this.source = source;
    }

    public void apply(float deltaTime)
    {
        subject.takeDamageNoObs(DEGENSPEED * deltaTime * multiple, source);
    }

    public float getEnd()
    { return endTime; }

    public void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public void end(ObjectActor subject)
    {
        multiple = 1;
        Destroy(this);
    }

    public int getMult()
    {
        return multiple;
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(1));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, name, description, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
        }
        addDuration(2.0f);
        string newName = ("Burning x" + multiple);
        string newDescription = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        bool timed = true;
        effectFunctions.iconUpdate(instanceList, newName, newDescription, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }

    public void addDuration(float addition)
    {
        endTime += addition;
        if (endTime - Time.time >= 17.0f)
        {
            endTime = Time.time + 17.0f;
        }
    }
}
