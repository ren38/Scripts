using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentencedEffect : MonoBehaviour, IEffect
{
    private const float SENTENCEDDURATION = 20.0f;
    private const float SPEEDLOSS = 0.25f;
    private int multiple = 1;
    private const int MAXMULTIPLIER = 3;
    private float speedChanged;
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
        Debug.Log("WARNING. SENTENCED EFFECT NOT FULLY IMPLEMENTED");
        instanceList = new List<GameObject>();
        timed = true;
        duration = SENTENCEDDURATION;
        endTime = duration + Time.time;
        effectName = "Crippled";
        description = string.Format("Speed reduced by {0}%.", SPEEDLOSS * 100);
        this.subject = subject;
        this.source = source;
        speedChanged = subject.moveSpeedChangePercent(-SPEEDLOSS);
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
        subject.moveSpeedChangeValue(speedChanged * multiple);
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
            subject.moveSpeedChangeValue(-speedChanged);
        }
        string newName = ("Crippled x" + multiple);
        string newDescription = string.Format("Speed reduced by {0}%.", (int)SPEEDLOSS * 100);
        bool timed = true;
        effectFunctions.iconUpdate(instanceList, newName, newDescription, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }
}