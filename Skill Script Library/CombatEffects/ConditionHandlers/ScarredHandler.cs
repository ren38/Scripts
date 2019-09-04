using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarredHandler : ConditionHandler
{
    public void beginScarred(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<ScarredEffect>(subject, source);
    }

    public ScarredEffect GetScarred()
    {
        return getCondition<ScarredEffect>();
    }

    public int EndScarred()
    {
        return endCondition<ScarredEffect>();
    }
}
