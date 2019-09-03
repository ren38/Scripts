using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionHandler<BaseCondition> : MonoBehaviour
{
    private List<effectObserver> conditionBeginObservers;
    private List<effectObserver> conditionStackObservers;

    public void beginCondition(ObjectInteractable source)
    {
        //check if there is already a bleed effect active.
        //if not, start one. else stack one
        BaseCondition preexisting = getCondition();
        if (preexisting == null)
        {
            BaseCondition condition = gameObject.AddComponent<BaseCondition>();
            condition.setup(this, source);
            applyNewEffect(bleeding);
            foreach (effectObserver obs in bleedBeginObservers)
            {
                obs.trigger(bleeding);
            }
        }
        else
        {
            preexisting.stack();
            foreach (effectObserver obs in bleedStackObservers)
            {
                obs.trigger(preexisting);
            }
        }
    }

    public int endBleed()
    {
        BleedEffect preexisting = getBleedEffect();
        if (preexisting != null)
        {
            int mult = preexisting.getMult();
            preexisting.abruptEnd();
            return mult;
        }
        return 0;
    }

    public ConditionEffect getCondition()
    {
        return gameObject.GetComponent<ConditionEffect>();
    }

    public void bleedBeginSubscribe(effectObserver observer)
    {
        if (!bleedBeginObservers.Contains(observer))
            bleedBeginObservers.Add(observer);
        observer.connect(bleedBeginObservers);
    }

    public void bleedStackSubscribe(effectObserver observer)
    {
        if (!bleedStackObservers.Contains(observer))
            bleedStackObservers.Add(observer);
        observer.connect(bleedStackObservers);
    }
}
