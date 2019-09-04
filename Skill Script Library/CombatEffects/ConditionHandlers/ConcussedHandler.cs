using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcussedHandler : ConditionHandler
{
    public void beginConcussed(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<ConcussedEffect>(subject, source);
    }

    public ConcussedEffect GetConcussed()
    {
        return getCondition<ConcussedEffect>();
    }

    public int EndConcussed()
    {
        return endCondition<ConcussedEffect>();
    }
}
