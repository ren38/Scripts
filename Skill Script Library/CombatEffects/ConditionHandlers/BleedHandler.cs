using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedHandler : ConditionHandler
{
    public void beginBleed(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<BleedEffect>(subject, source);
    }

    public BleedEffect GetBleed()
    {
        return getCondition<BleedEffect>();
    }

    public int endCondition()
    {
        return endCondition<BleedEffect>();
    }
}
