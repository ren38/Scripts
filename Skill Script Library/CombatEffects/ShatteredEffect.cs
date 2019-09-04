using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredEffect : BaseCondition
{
    private const float SHATTEREDDURATION = 3.0f;
    private const int ARMORLOSS = 20;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = SHATTEREDDURATION;
        effectName = "Shattered";
        description = string.Format("Physical armor value reduced by {0}.", ARMORLOSS);
        MAXMULTIPLIER = 3;
        base.setup(subject, source);
        subject.physicalArmorValueChange(-ARMORLOSS);
    }

    public override void end(ObjectActor subject)
    {
        subject.physicalArmorValueChange(ARMORLOSS * multiple);
        base.end(subject);
    }

    public override void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
            subject.physicalArmorValueChange(-ARMORLOSS);
        }
        effectName = ("Shattered x" + multiple);
        description = string.Format("Physical armor value reduced by {0}.", ARMORLOSS * multiple);
        base.stack();
    }
}
