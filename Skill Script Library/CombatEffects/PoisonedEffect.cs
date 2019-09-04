using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedEffect : BaseCondition
{
    private const float POISONDURATION = 10.0f;
    private const float DEGENSPEED = 4.0f;

    public override void setup(ObjectActor subject, ObjectInteractable source)
    {
        duration = POISONDURATION;
        effectName = "Poisoned";
        description = string.Format("Lose {0} health per second.", DEGENSPEED);
        base.setup(subject, source);
    }

    public override void apply(float deltaTime)
    {
        subject.takeDamageNoObs(DEGENSPEED * deltaTime, source);
    }

    public override void stack()
    {
        endTime += duration;// no max duration
        base.stack();
    }
}
