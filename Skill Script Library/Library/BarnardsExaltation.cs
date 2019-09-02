using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnardsExaltation : BaseSkill, IEffect
{
    [SerializeField]
    protected float REGENSPEED = 10.0f;
    [SerializeField]
    protected float DEGENSPEED = 15.0f;
    protected ObjectInteractable source;
    protected ObjectActor subject;

    protected List<GameObject> instanceList;
    protected const bool timed = true;
    [SerializeField]
    protected float duration;
    protected float endTime;

    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        if (targetActor != null)
        {
            BarnardsExaltation preexisting = target.gameObject.GetComponent<BarnardsExaltation>();
            if (preexisting == null)
            {
                BarnardsExaltation regeneration = target.gameObject.AddComponent<BarnardsExaltation>();
                regeneration.setup(targetActor, source, skillName, briefSkillDescription, duration, REGENSPEED, DEGENSPEED);
                targetActor.applyNewEffect(regeneration);
            }
            else
            {
                preexisting.stack();
            }
        }
    }

    public void setup(
        ObjectActor subject, ObjectInteractable source,
        string name, string desc, float duration, float regen, float degen
        )
    {
        this.subject = subject; // order passed in.
        this.source = source;
        skillName = name;
        briefSkillDescription = desc;
        this.duration = duration;
        this.REGENSPEED = regen;
        this.DEGENSPEED = degen;
        setEnd(duration);
        instanceList = new List<GameObject>();
    }

    public void apply(float deltaTime)
    {
        if (subject.gameObject.GetComponent<BleedEffect>() != null)
        {
            subject.takeHealingNoObs(REGENSPEED * deltaTime, source);
        }
        else
        {
            subject.takeDamageNoObs(DEGENSPEED * deltaTime, source);
        }
    }

    public float getEnd()
    {
        return endTime;
    }

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
        GameObject newInstance = Instantiate(SkillLibrary.Instance.getByID(6));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, skillName, briefSkillDescription, timed, endTime);
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
