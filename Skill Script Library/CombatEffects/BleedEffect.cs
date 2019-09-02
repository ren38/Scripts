using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedEffect : MonoBehaviour, IEffect
{
    private const float BLEEDDURATION = 10.0f;
    private const float DEGENSPEED = 2.0f;
    private int multiple = 1;
    private const int MAXMULTIPLIER = 10;
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
        duration = BLEEDDURATION;
        endTime = duration + Time.time;
        effectName = "Bleed";
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
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(0));
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
        endTime += duration;
        if(endTime - Time.time >= 15.0f)
        {
            endTime = Time.time + 15.0f;
        }
        string newName = ("Bleed x" + multiple);
        string newDescription = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        bool timed = true;
        effectFunctions.iconUpdate(instanceList, newName, newDescription, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }
}
