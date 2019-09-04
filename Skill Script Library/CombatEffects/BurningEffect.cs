using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : BaseCondition
{
    private const float BURNINGDURATION = 3.0f;
    private const float DEGENSPEED = 7.0f;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        effectName = "Burning";
        description = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        conditionID = 1;
        duration = BURNINGDURATION;
        multiple = 1;
        MAXMULTIPLIER = 3;
        base.setup(subject, source);
    }

    public override void apply(float deltaTime)
    {
        subject.takeDamageNoObs(DEGENSPEED * deltaTime * multiple, source);
    }

    public override void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
        }
        addDuration(2.0f);
        effectName = ("Burning x" + multiple);
        description = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        base.stack();
    }

    public void addDuration(float addition)
    {
        endTime += addition;
        if (endTime - Time.time >= 17.0f)
        {
            endTime = Time.time + 17.0f;
        }
    }
}
