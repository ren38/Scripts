using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundedEffect : BaseCondition
{
    private const float WOUNDEDDURATION = 3.0f;
    private const float HEALTHLOSS = 30.0f;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = WOUNDEDDURATION;
        endTime = duration + Time.time;
        effectName = "Wounded";
        description = string.Format("Maximum health reduced by {0}.", HEALTHLOSS);
        subject.changeMaxHealth(-HEALTHLOSS);
        base.setup(subject, source);
    }

    public override void end(ObjectActor subject)
    {
        subject.changeMaxHealth(HEALTHLOSS);
        Destroy(this);
    }
}
