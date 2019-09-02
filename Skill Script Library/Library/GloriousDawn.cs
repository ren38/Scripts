using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloriousDawn : BaseSkill, IEffect
{
    /*
        skillName = "Glorious Dawn";
        briefSkillDescription = "Target is healed for 10 over 5 seconds.";
        fullSkillDescription = "Target gains 2 health per second over 5 seconds.";
        energyCost = 10.0f;
        sacrificeCost = 0.0f;
        adrenalineCost = 0;
        castTime = 0.5f;
        cooldown = 2.5f;
        range = 6.5f;
    */
    [SerializeField]
    protected float REGENSPEED = 20.0f;
    protected ObjectInteractable source;
    protected ObjectActor subject;

    protected List<GameObject> instanceList;
    protected const bool timed = true;
    [SerializeField]
    protected float duration;
    protected float endTime;
    [SerializeField]
    protected float stackHealing = 30.0f;


    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        if (targetActor != null)
        {
            GloriousDawn preexisting = target.gameObject.GetComponent<GloriousDawn>();
            if (preexisting == null)
            {
                GloriousDawn regeneration = target.gameObject.AddComponent<GloriousDawn>();
                regeneration.setup(targetActor, source, skillName, briefSkillDescription, duration, stackHealing);
                targetActor.applyNewEffect(regeneration);
                targetActor.beginBurning(self);
            }
            else
            {
                preexisting.stack();
            }
        }
    }

    public void setup(
        ObjectActor subject, ObjectInteractable source, 
        string name, string desc, float duration, float stackHealing
        )
    {
        this.subject = subject; // order passed in.
        this.source = source;
        skillName = name;
        briefSkillDescription = desc;
        this.duration = duration;
        this.stackHealing = stackHealing;
        setEnd(duration);
        instanceList = new List<GameObject>();
    }

    public void apply(float deltaTime)
    {
        subject.takeHealingNoObs(REGENSPEED * deltaTime, source);
    }

    public float getEnd()
    {
        return endTime; }

    public void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public void end(ObjectActor subject)
    {
        clearIconInstances();
        Destroy(this);
    }

    public void clearIconInstances()
    {
        foreach (GameObject obj in instanceList)
        {
            Destroy(obj);
        }
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(SkillLibrary.Instance.getByID(3));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, skillName, briefSkillDescription, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        subject.takeRadiantHealing(stackHealing, source);
    }

    public float getDuration()
    {
        return duration;
    }
}
