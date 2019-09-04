using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCondition : MonoBehaviour, IEffect
{
    protected string effectName;
    protected string description;
    protected bool timed;
    protected float duration;
    protected float endTime;
    protected int conditionID;
    protected int multiple;
    protected int MAXMULTIPLIER;
    protected List<GameObject> instanceList;
    protected ObjectActor subject;
    protected ObjectInteractable source;

    public virtual void setup(ObjectActor subject, ObjectInteractable source)
    {
        //setup contains all the elements that are common to all conditions
        instanceList = new List<GameObject>();
        timed = true;
        endTime = duration + Time.time;
        this.subject = subject;
        this.source = source;
    }

    public virtual void apply(float deltaTime)
    {
        return;
    }

    public virtual void end(ObjectActor subject)
    {
        Destroy(this);
    }

    public virtual float getDuration()
    {
        return duration;
    }

    public int getMult()
    {
        return multiple;
    }

    public virtual float getEnd(){ return endTime; }

    public virtual GameObject getIcon()
    {
        if(instanceList == null)
        {
            Debug.Log("Warning! instance list has not been initialized.");
            return null;
        }
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(conditionID));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, effectName, description, timed, endTime);
        return newInstance;
    }

    public virtual void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public virtual void stack()
    {
        effectFunctions.iconUpdate(instanceList, effectName, description, timed, endTime);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
    }

}
