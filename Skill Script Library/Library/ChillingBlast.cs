using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillingBlast : BaseSkill
{
    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        ObjectActor targetActor = target as ObjectActor;
        target.takeColdDamage(10.0f, self);
        if (targetActor != null)
        {
            targetActor.beginChilled(self);
        }
    }
}
