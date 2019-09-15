using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField]
    protected OrderEnum order;
    [SerializeField]
    protected string skillName;
    [TextArea][SerializeField]
    protected string briefSkillDescription;
    [TextArea][SerializeField]
    protected string fullSkillDescription;
    [SerializeField]
    protected float energyCost;
    [SerializeField]
    protected float sacrificeCost;
    [SerializeField]
    protected int adrenalineCost;
    [SerializeField]
    protected float castTime;
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected float range;
    [SerializeField]
    private int ID;
    [SerializeField]
    protected List<skillType> types;

    public void setID(int newID){ID = newID;}
    public int getID() { return ID; }

    // activate returns true if a message is needed due to something happening.
    // An example is a condition for a skill being activated is not met. The message string is filled
    // with the text to be shown to the player.
    public abstract bool activate(ObjectActor self, ObjectCombatable target, out string message);
    //public abstract bool validateTargetting(ObjectCombatable target);
    public virtual OrderEnum getOrder() { return order; }
    public virtual float getCooldown() { return cooldown; }
    public virtual float getCastTime() { return castTime; }
    public virtual float getEnergyCost() { return energyCost; }
    public virtual float getHealthCost() { return sacrificeCost; }
    public virtual float getAdrenalCost() { return adrenalineCost; }
    public virtual float getRange() { return range; }
    public virtual string getName() { return skillName; }
    public virtual string getShortDescription() { return briefSkillDescription; }
    public virtual string getLongDescription() { return fullSkillDescription; }
    public virtual List<skillType> getTypes() { return types; }
}
