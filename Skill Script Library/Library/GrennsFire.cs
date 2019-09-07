using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrennsFire : BaseSkill, IEffect
{
    [SerializeField]
    protected float DEGENSPEED = 2.0f;
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
            GrennsFire preexisting = target.gameObject.GetComponent<GrennsFire>();
            if (preexisting == null)
            {
                GrennsFire grennsFire = target.gameObject.AddComponent<GrennsFire>();
                grennsFire.setup(targetActor, source, skillName, briefSkillDescription, duration, DEGENSPEED);
                targetActor.applyNewEffect(grennsFire);
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
        string name, string desc, float duration, float DEGENSPEED
        )
    {
        this.subject = subject; // order passed in.
        this.source = source;
        skillName = name;
        briefSkillDescription = desc;
        this.duration = duration;
        this.DEGENSPEED = DEGENSPEED;
        setEnd(duration);
        instanceList = new List<GameObject>();
    }

    public void apply(float deltaTime)
    {
        if(subject.getBurningEffect() != null)
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
        subject.endBurning();
        subject.beginShattered(source);
        Destroy(this);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
    }


    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(SkillLibrary.Instance.getByID(10));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, skillName, briefSkillDescription, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        if (subject.getBurningEffect() != null)
        {
            subject.beginBurning(source);
            subject.beginWounded(source);
        }
    }

    public float getDuration()
    {
        return duration;
    }
}
