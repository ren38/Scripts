using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedEffect : MonoBehaviour, IEffect
{
    private const float POISONDURATION = 10.0f;
    private const float DEGENSPEED = 4.0f;
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
        duration = POISONDURATION;
        endTime = duration + Time.time;
        effectName = "Bleed";
        description = string.Format("Lose {0} health per second.", DEGENSPEED);
        this.subject = subject;
        this.source = source;
    }

    public void apply(float deltaTime)
    {
        subject.takeDamageNoObs(DEGENSPEED * deltaTime, source);
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

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(5));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, name, description, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        endTime += duration;// no max duration
        effectFunctions.iconUpdate(instanceList, effectName, description, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }
}
