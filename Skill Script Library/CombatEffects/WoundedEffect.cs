using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundedEffect : MonoBehaviour, IEffect
{
    private const float WOUNDEDDURATION = 3.0f;
    private const float HEALTHLOSS = 30.0f;
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
        duration = WOUNDEDDURATION;
        endTime = duration + Time.time;
        effectName = "Arcane Scarred";
        description = string.Format("Maximum health reduced by {0}.", HEALTHLOSS);
        this.subject = subject;
        this.source = source;
        subject.changeMaxHealth(-HEALTHLOSS);
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
        subject.changeMaxHealth(HEALTHLOSS);
        Destroy(this);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(9));
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
