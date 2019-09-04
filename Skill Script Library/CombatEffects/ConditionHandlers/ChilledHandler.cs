using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChilledHandler : ConditionHandler
{
    public void beginChilled(ObjectActor subject, ObjectInteractable source)
    {
        BurningEffect burning = subject.getBurningEffect();
        if (burning != null)
        {
            subject.endBurning();
            return;
        }
        beginCondition<ChilledEffect>(subject, source);
    }

    public ChilledEffect GetChilled()
    {
        return getCondition<ChilledEffect>();
    }

    public int EndChilled()
    {
        return endCondition<ChilledEffect>();
    }
}
