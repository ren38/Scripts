using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenedEffect : BaseCondition
{
    private const float WEAKENEDDURATION = 30.0f;
    private const float ATTACKLOSS = 0.3f;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = WEAKENEDDURATION;
        MAXMULTIPLIER = 3;
        effectName = "Weakened";
        description = string.Format("Basic attack power reduced by {0}%.", ATTACKLOSS * 100);
        subject.attackModChange(-ATTACKLOSS);
        base.setup(subject, source);
    }

    public override void end(ObjectActor subject)
    {
        subject.attackModChange(ATTACKLOSS * multiple);
        base.end(subject);
    }

    public override void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
            subject.attackModChange(-ATTACKLOSS);
        }
        effectName = ("Weakened x" + multiple);
        description = string.Format("Basic attack power reduced by {0}%.", ATTACKLOSS * multiple * 100);
        base.stack();
    }
}

