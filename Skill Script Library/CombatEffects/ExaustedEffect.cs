using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustedEffect : MonoBehaviour, IEffect
{
    private const float EXHAUSTEDDURATION = 3.0f;
    private const float ENERGYLOSS = 30.0f;
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
        duration = EXHAUSTEDDURATION;
        endTime = duration + Time.time;
        effectName = "Exhausted";
        description = string.Format("Maximum Energy reduced by {0}.", ENERGYLOSS);
        this.subject = subject;
        this.source = source;
        subject.changeMaxEnergy(-ENERGYLOSS);
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
        subject.changeMaxEnergy(ENERGYLOSS);
        Destroy(this);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
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
        return;
    }

    public float getDuration()
    {
        return duration;
    }
}
