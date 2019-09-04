using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarredEffect : BaseCondition
{
    private const float SCARREDDURATION = 3.0f;
    private const int ARMORLOSS = 30;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = SCARREDDURATION;
        effectName = "Arcane Scarred";
        description = string.Format("Magic armor value decreased by {0}.", ARMORLOSS);
        MAXMULTIPLIER = 3;
        subject.magicArmorValueChange(-ARMORLOSS);
        conditionID = 7;
        base.setup(subject, source);
    }

    public override void end(ObjectActor subject)
    {
        subject.magicArmorValueChange(ARMORLOSS * multiple);
        Destroy(this);
    }

    public override void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
            subject.magicArmorValueChange(-ARMORLOSS);
        }
        effectName = ("Arcane Scarred x" + multiple);
        description = string.Format("Magic armor value reduced by {0}.", ARMORLOSS * multiple);
        base.stack();
    }
}
