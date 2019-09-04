using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionHandler : MonoBehaviour
{
    private List<effectObserver> conditionBeginObservers;
    private List<effectObserver> conditionStackObservers;

    public void beginCondition<condition>(ObjectActor subject, ObjectInteractable source) where condition : BaseCondition
    {
        
        //Break this up into smaller functions.

        BaseCondition preexisting = getCondition();
        if (preexisting == null)
        {
            newInstance<condition>(subject, source);
        }
        else
        {
            applyStack(preexisting);
        }
    }

    protected void newInstance<condition>(ObjectActor subject, ObjectInteractable source) where condition : BaseCondition
    {
        condition newCondition = gameObject.AddComponent<condition>();
        newCondition.setup(subject, source);
        subject.applyNewEffect(newCondition);
        foreach (effectObserver obs in conditionBeginObservers)
        {
            obs.trigger(newCondition);
        }
    }

    protected void applyStack(BaseCondition preexisting)
    {
        preexisting.stack();
        foreach (effectObserver obs in conditionStackObservers)
        {
            obs.trigger(preexisting);
        }
    }

    public int endCondition()
    {
        BaseCondition preexisting = getCondition();
        if (preexisting != null)
        {
            preexisting.abruptEnd();
        }
        return 0;
    }

    public BaseCondition getCondition()
    {
        return gameObject.GetComponent<BaseCondition>();
    }

    public void conditionBeginSubscribe(effectObserver observer)
    {
        if (!conditionBeginObservers.Contains(observer))
            conditionBeginObservers.Add(observer);
        observer.connect(conditionBeginObservers);
    }

    public void conditionStackSubscribe(effectObserver observer)
    {
        if (!conditionStackObservers.Contains(observer))
            conditionStackObservers.Add(observer);
        observer.connect(conditionStackObservers);
    }
}
