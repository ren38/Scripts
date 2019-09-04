using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedHandler : ConditionHandler
{
    public void beginPoisoned(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<PoisonedEffect>(subject, source);
    }

    public PoisonedEffect GetPoisoned()
    {
        return getCondition<PoisonedEffect>();
    }

    public int EndPoisoned()
    {
        return endCondition<PoisonedEffect>();
    }
}
