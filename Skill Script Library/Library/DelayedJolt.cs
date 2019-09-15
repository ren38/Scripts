using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedJolt : BaseSkill, IEffect
{
    [SerializeField]
    protected float Damage = 40.0f;
    protected ObjectInteractable source;
    protected ObjectActor subject;

    protected List<GameObject> instanceList;
    protected const bool timed = true;
    [SerializeField]
    protected float duration = 20.0f;
    protected float endTime;
    protected ObjectActor sourceActor;


    public override bool activate(ObjectActor self, ObjectCombatable target, out string message)
    {
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        sourceActor = self;
        if (targetActor != null)
        {
            DelayedJolt preexisting = target.gameObject.GetComponent<DelayedJolt>();
            if (preexisting == null)
            {
                DelayedJolt regeneration = target.gameObject.AddComponent<DelayedJolt>();
                regeneration.setup(targetActor, source, sourceActor, skillName, briefSkillDescription, duration);
                targetActor.applyNewEffect(regeneration);
            }
            else
            {
                preexisting.stack();
            }
        }
        message = "";
        return false;
    }

    public void setup(
        ObjectActor subject, ObjectInteractable source, ObjectActor sourceActor,
        string name, string desc, float duration
        )
    {
        this.subject = subject; // order passed in.
        this.source = source;
        this.sourceActor = sourceActor;
        skillName = name;
        briefSkillDescription = desc;
        this.duration = duration;
        setEnd(duration);
        instanceList = new List<GameObject>();
    }

    public void apply(float deltaTime)
    {
        return;
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
        float damage = sourceActor.getStr() * 0.5f + 40.0f;
        subject.takeElectricDamage(damage, (ObjectInteractable)source);
        Destroy(this);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
    }


    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(SkillLibrary.Instance.getByID(7));
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
