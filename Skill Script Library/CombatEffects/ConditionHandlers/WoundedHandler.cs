using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundedHandler : ConditionHandler
{
    public void beginWounded(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<WoundedEffect>(subject, source);
    }

    public WoundedEffect GetWounded()
    {
        return getCondition<WoundedEffect>();
    }

    public int EndWounded()
    {
        return endCondition<WoundedEffect>();
    }
}
