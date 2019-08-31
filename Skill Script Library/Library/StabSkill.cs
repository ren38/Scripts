using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabSkill : BaseSkill
{
    /*
        skillName = "Stab";
        briefSkillDescription = "Target takes 20 piercing damage.";
        fullSkillDescription = "Target is struck from up to 3.5 meters away for 20 piercing damage.";
        energyCost = 5.0f;
        sacrificeCost = 0.02f;
        adrenalineCost = 1;
        castTime = 0.2f;
        cooldown = 5.5f;
        range = 3.5f;
    */

    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        float damage = self.getStr() * 0.5f + 10.0f;
        target.takePiercingDamage(damage, (ObjectInteractable)self);
    }
}
