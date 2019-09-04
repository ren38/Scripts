using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrippledEffect : BaseCondition
{
    private const float CRIPPLEDDURATION = 20.0f;
    private const float SPEEDLOSS = 0.25f;
    private float speedChanged;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = CRIPPLEDDURATION;
        effectName = "Crippled";
        description = string.Format("Speed reduced by {0}%.", SPEEDLOSS * 100);
        MAXMULTIPLIER = 3;
        speedChanged = subject.moveSpeedChangePercent(-SPEEDLOSS);
        conditionID = 8;
        base.setup(subject, source);
    }
    public override void end(ObjectActor subject)
    {
        subject.moveSpeedChangeValue(speedChanged * multiple);
        Destroy(this);
    }

    public override void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
            subject.moveSpeedChangeValue(-speedChanged);
        }
        effectName = ("Crippled x" + multiple);
        description = string.Format("Speed reduced by {0}%.", (int)SPEEDLOSS * 100);
        base.stack();
    }
}