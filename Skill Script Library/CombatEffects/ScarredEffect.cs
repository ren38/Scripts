using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarredEffect : MonoBehaviour, IEffect
{
    private const float SCARREDDURATION = 3.0f;
    private const int ARMORLOSS = 30;
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
        duration = SCARREDDURATION;
        endTime = duration + Time.time;
        effectName = "Arcane Scarred";
        description = string.Format("Magic armor value decreased by {0}.", ARMORLOSS);
        this.subject = subject;
        this.source = source;
        subject.magicArmorValueChange(-ARMORLOSS);
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
        subject.magicArmorValueChange(ARMORLOSS * multiple);
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
        GameObject newInstance = Instantiate(ConditionLibrary.Instance.getInstanceByID(7));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, name, description, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
            subject.magicArmorValueChange(-ARMORLOSS);
        }
        string newName = ("Arcane Scarred x" + multiple);
        string newDescription = string.Format("Magic armor value reduced by {0}.", ARMORLOSS * multiple);
        bool timed = true;
        effectFunctions.iconUpdate(instanceList, newName, newDescription, timed, endTime);
    }

    public float getDuration()
    {
        return duration;
    }
}
