using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExactSlash : BaseSkill
{
    /*
        skillName = "Exact Slash";
        briefSkillDescription = "Target suffers bleeding.";
        fullSkillDescription = "Target suffers bleeding for 10 seconds.";
        energyCost = 10.0f;
        sacrificeCost = 0.0f;
        adrenalineCost = 0;
        castTime = 0.5f;
        cooldown = 1.5f;
        range = 3.0f;
    */

    public override bool activate(ObjectActor self, ObjectCombatable target, out string message)
    {
        ObjectActor targetActor = target as ObjectActor;

        if (targetActor != null)
        {
            targetActor.beginBleeding(self);
        }
        message = "";
        return false;
    }
}
