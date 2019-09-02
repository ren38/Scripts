using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChilledEffect : MonoBehaviour, IEffect
{
    private const float CHILLEDDURATION = 20.0f;
    private float SPEEDLOSS = 0.1f;
    protected ObjectActor subject;
    protected ObjectInteractable source;
    protected FloatAdjuster obs;

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
        duration = CHILLEDDURATION;
        endTime = duration + Time.time;
        effectName = "Burning";
        description = string.Format("Skill activation takes {0}x longer.", 1.0f + SPEEDLOSS);
        this.subject = subject;
        this.source = source;
        obs = subject.gameObject.AddComponent<FloatAdjuster>();
        obs.setupObserver(change);
        subject.skillStartSubscribe(obs);
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

    public float change(float num)
    {
        return num + SPEEDLOSS;
    }

    public void end(ObjectActor subject)
    {
        Destroy(this);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
    }

    public float getSpeedloss()
    {
        return SPEEDLOSS;
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(2));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, name, description, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        addDuration(8.0f);
        addLoss(0.1f);
        string newName = ("Chilled x" + (1.0f + SPEEDLOSS));
        string newDescription = string.Format("Skill activation takes {0}x longer.", 1.0f + SPEEDLOSS);
        bool timed = true;
        effectFunctions.iconUpdate(instanceList, newName, newDescription, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }

    public void addLoss(float addition)
    {
        SPEEDLOSS += addition;
        if (SPEEDLOSS >= 2.0f)
        {
            SPEEDLOSS = 2.0f;
        }
    }

    public void addDuration(float addition)
    {
        endTime += addition;
        if (endTime - Time.time >= 60.0f)
        {
            endTime = Time.time + 60.0f;
        }
    }
}
