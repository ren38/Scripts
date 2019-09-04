using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustedHandler : ConditionHandler
{
    public void beginExhausted(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<ExhaustedEffect>(subject, source);
    }

    public ExhaustedEffect GetExhausted()
    {
        return getCondition<ExhaustedEffect>();
    }

    public int EndExhausted()
    {
        return endCondition<ExhaustedEffect>();
    }
}
