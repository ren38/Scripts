using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCondition : BaseSkill
{
    public override bool activate(ObjectActor self, ObjectCombatable target, out string message)
    {
        ObjectActor actor = (ObjectActor)target;
        if(actor != null)
        {
            actor.beginBleeding(self);
            actor.beginBurning(self);
            actor.beginChilled(self);
            actor.beginConcussed(self);
            actor.beginCrippled(self);
            actor.beginExhausted(self);
            actor.beginPoisoned(self);
            actor.beginScarred(self);
            actor.beginShattered(self);
            actor.beginWeakened(self);
            actor.beginWounded(self);
        }
        message = "";
        return false;
    }
}
