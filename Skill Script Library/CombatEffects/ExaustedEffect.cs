using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustedEffect : BaseCondition
{
    private const float EXHAUSTEDDURATION = 3.0f;
    private const float ENERGYLOSS = 30.0f;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = EXHAUSTEDDURATION;
        effectName = "Exhausted";
        description = string.Format("Maximum Energy reduced by {0}.", ENERGYLOSS);
        subject.changeMaxEnergy(-ENERGYLOSS);
        conditionID = 10;
        base.setup(subject, source);
    }

    public override void end(ObjectActor subject)
    {
        subject.changeMaxEnergy(ENERGYLOSS);
        Destroy(this);
    }
}
