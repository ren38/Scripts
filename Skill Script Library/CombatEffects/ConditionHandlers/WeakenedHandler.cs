using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenedHandler : ConditionHandler
{
    public void beginWeakened(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<WeakenedEffect>(subject, source);
    }

    public WeakenedEffect GetWeakened()
    {
        return getCondition<WeakenedEffect>();
    }

    public int EndWeakened()
    {
        return endCondition<WeakenedEffect>();
    }
}
