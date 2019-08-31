using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyphersPoisonSkill : BaseSkill, IEffect
{
    /*
    skillName = "Cypher's Poison";
    briefSkillDescription = "Target takes 15 extra damage.";
    fullSkillDescription = "While this enchantment is active, the target " +
    "takes 15 extra damage each time damage is applied. (Does not count damage over time.)";
    energyCost = 15.0f;
    sacrificeCost = 0.0f;
    adrenalineCost = 0;
    castTime = 1.5f;
    cooldown = 5.5f;
    range = 15.0f;
    */

    protected ObjectInteractable source;
    protected ObjectActor subject;
    protected FloatAdjuster obs;

    protected List<GameObject> instanceList;
    protected bool timed = true;
    [SerializeField]
    protected float duration;
    protected float endTime;
    [SerializeField]
    private float damage = 15.0f;


    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        //Debug.Log(energyCost);
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        if (targetActor != null)
        {
            CyphersPoisonSkill preexisting = target.gameObject.GetComponent<CyphersPoisonSkill>();
            if (preexisting == null)
            {
                CyphersPoisonSkill cyphers = target.gameObject.AddComponent<CyphersPoisonSkill>();
                cyphers.setup(targetActor, source, skillName, briefSkillDescription, duration, damage);
                targetActor.applyNewEffect(cyphers);
            }
            else
            {
                preexisting.stack();
            }
        }
    }

    public void setup(
        ObjectActor subject, ObjectInteractable source,
        string name, string desc, float duration, float damage
        )
    {
        this.subject = subject; // order passed in.
        this.source = source;
        skillName = name;
        briefSkillDescription = desc;
        this.duration = duration;
        this.damage = damage;
        setEnd(duration);
        obs = subject.gameObject.AddComponent<FloatAdjuster>();
        obs.setupObserver(addDamage);
        subject.rawDamageSubscribe(obs);
        instanceList = new List<GameObject>();
    }

    public void apply(float deltaTime) //, ObjectInteractable source
    {
        return; // this does not apply every frame.
    }

    public float getEnd()
    { return endTime; }

    public void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public void end(ObjectActor subject)
    {
        obs.complete();
        Destroy(this);
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(SkillLibrary.Instance.getByID(2));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, skillName, briefSkillDescription, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        endTime = endTime + duration;
        effectFunctions.iconUpdate(instanceList, skillName, briefSkillDescription, true, endTime);
    }

    public float getDuration()
    {
        return duration;
    }

    private float addDamage(float baseDmg)
    {
        return baseDmg + damage;
    }
}
