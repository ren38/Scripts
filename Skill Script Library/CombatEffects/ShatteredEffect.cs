using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredEffect : MonoBehaviour, IEffect
{
    private const float SHATTEREDDURATION = 3.0f;
    private const int ARMORLOSS = 20;
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
        duration = SHATTEREDDURATION;
        endTime = duration + Time.time;
        effectName = "Shattered";
        description = string.Format("Physical armor value reduced by {0}.", ARMORLOSS);
        this.subject = subject;
        this.source = source;
        subject.physicalArmorValueChange(-ARMORLOSS);
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
        subject.physicalArmorValueChange(ARMORLOSS * multiple);
        Destroy(this);
    }

    public int getMult()
    {
        return multiple;
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(3));
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
        subject.physicalArmorValueChange(-ARMORLOSS);
        string newName = ("Shattered x" + multiple);
        string newDescription = string.Format("Physical armor value reduced by {0}.", ARMORLOSS * multiple);
        bool timed = true;
        effectFunctions.iconUpdate(instanceList, newName, newDescription, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }
}
