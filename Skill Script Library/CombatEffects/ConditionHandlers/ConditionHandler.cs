using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionHandler : MonoBehaviour
{
    private List<effectObserver> conditionBeginObservers;
    private List<effectObserver> conditionStackObservers;

    public void setupHandler()
    {
        conditionBeginObservers = new List<effectObserver>();
        conditionStackObservers = new List<effectObserver>();
    }

    public void beginCondition<Condition>(ObjectActor subject, ObjectInteractable source) where Condition : BaseCondition
    {
        
        //Break this up into smaller functions.

        BaseCondition preexisting = getCondition<Condition>();
        if (preexisting == null)
        {
            newInstance<Condition>(subject, source);
        }
        else
        {
            applyStack(preexisting);
        }
    }

    protected void newInstance<Condition>(ObjectActor subject, ObjectInteractable source) where Condition : BaseCondition
    {
        Condition newCondition = gameObject.AddComponent<Condition>();
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

    public int endCondition<condition>() where condition : BaseCondition
    {
        condition preexisting = getCondition<condition>();
        if (preexisting != null)
        {
            int mult = preexisting.getMult();
            preexisting.abruptEnd();
            return mult;
        }
        return 0;
    }

    public condition getCondition<condition>() where condition : BaseCondition
    {
        return gameObject.GetComponent<condition>();
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
