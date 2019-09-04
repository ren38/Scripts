using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningHandler : ConditionHandler
{
    public void beginBurning(ObjectActor subject, ObjectInteractable source)
    {
        ChilledEffect chilled = subject.getChilledEffect();
        if (chilled != null)
        {
            subject.endChilled();
            return;
        }
        beginCondition<BurningEffect>(subject, source);
    }

    public BurningEffect GetBurning()
    {
        return getCondition<BurningEffect>();
    }

    public int EndBurning()
    {
        return endCondition<BurningEffect>();
    }
}
