using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BaseSkill, IEffect
{

    protected ObjectInteractable source;
    protected ObjectActor subject;
    protected FloatAdjuster obs1;
    protected FloatAdjuster obs2;
    protected FloatAdjuster obs3;

    protected List<GameObject> instanceList;
    protected bool timed = true;
    [SerializeField]
    protected float duration;
    protected float endTime;
    [SerializeField]
    private float value = 15.0f;

    public override bool activate(ObjectActor self, ObjectCombatable target, out string message)
    {
        //Debug.Log(energyCost);
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        if (targetActor != null)
        {
            Shield preexisting = target.gameObject.GetComponent<Shield>();
            if (preexisting == null)
            {
                Shield effect = target.gameObject.AddComponent<Shield>();
                effect.setup(targetActor, source, skillName, briefSkillDescription, duration, value);
                targetActor.applyNewEffect(effect);
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
        ObjectActor subject, ObjectInteractable source,
        string name, string desc, float duration, float value
        )
    {
        this.subject = subject; // order passed in.
        this.source = source;
        skillName = name;
        briefSkillDescription = desc;
        this.duration = duration;
        this.value = value;
        setEnd(duration);
        obs1 = subject.gameObject.AddComponent<FloatAdjuster>();
        obs1.setupObserver(subtractDamage);
        subject.piercingDamageSubscribe(obs1);
        obs2 = subject.gameObject.AddComponent<FloatAdjuster>();
        obs2.setupObserver(subtractDamage);
        subject.bludgeoningDamageSubscribe(obs2);
        obs3 = subject.gameObject.AddComponent<FloatAdjuster>();
        obs3.setupObserver(subtractDamage);
        subject.slashingDamageSubscribe(obs3);
        instanceList = new List<GameObject>();
    }

    public void apply(float deltaTime) //, ObjectInteractable source
    {
        //this.source = source;
        //subject.takeRawDamage(15.0f, source);
    }

    public float getEnd()
    { return endTime; }

    public void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public void end(ObjectActor subject)
    {
        obs1.complete();
        obs2.complete();
        obs3.complete();
        Destroy(this);
    }

    public void abruptEnd()
    {
        endTime = Time.time;
    }


    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(SkillLibrary.Instance.getByID(4));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, skillName, briefSkillDescription, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        endTime = duration + Time.time;
        effectFunctions.iconUpdate(instanceList, skillName, briefSkillDescription, true, endTime);
    }

    public float getDuration()
    {
        return duration;
    }

    private float subtractDamage(float baseDmg)
    {
        if (baseDmg < value)
        {
            return 0;
        }
        return baseDmg - value;
    }
}
