using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillingBlast : BaseSkill
{
    public override bool activate(ObjectActor self, ObjectCombatable target, out string message)
    {
        ObjectActor targetActor = target as ObjectActor;
        target.takeColdDamage(10.0f, self);
        if (targetActor != null)
        {
            targetActor.beginChilled(self);
        }
        message = "";
        return false;
    }
}
