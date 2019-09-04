using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrippledHandler : ConditionHandler
{
    public void beginCrippled(ObjectActor subject, ObjectInteractable source)
    {
        beginCondition<CrippledEffect>(subject, source);
    }

    public CrippledEffect GetCrippled()
    {
        return getCondition<CrippledEffect>();
    }

    public int EndCrippled()
    {
        return endCondition<CrippledEffect>();
    }
}
