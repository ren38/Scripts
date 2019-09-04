using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedEffect : BaseCondition
{
    private const float BLEEDDURATION = 10.0f;
    private const float DEGENSPEED = 2.0f;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        effectName = "Bleed";
        description = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        conditionID = 0;
        duration = BLEEDDURATION;
        multiple = 1;
        MAXMULTIPLIER = 10;
        base.setup(subject, source);
    }

    public override void apply(float deltaTime)
    {
        subject.takeDamageNoObs(DEGENSPEED * deltaTime * multiple, source);
    }

    public int getMult()
    {
        return multiple;
    }

    public override void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
        }
        endTime += duration;
        if(endTime - Time.time >= 15.0f)
        {
            endTime = Time.time + 15.0f;
        }
        effectName = ("Bleed x" + multiple);
        description = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        base.stack();
    }
}
