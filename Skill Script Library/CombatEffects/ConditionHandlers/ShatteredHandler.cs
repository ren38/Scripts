using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredHandler : ConditionHandler
{
    public void beginShattered(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<ShatteredEffect>(subject, source);
    }

    public ShatteredEffect GetShattered()
    {
        return getCondition<ShatteredEffect>();
    }

    public int EndShattered()
    {
        return endCondition<ShatteredEffect>();
    }
}
