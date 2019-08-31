using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Adjuster<TType> : MonoBehaviour
{
    public delegate TType1 Func<TType1>(TType1 datum);
    protected Func<TType> activate;
    private List<Adjuster<TType>> observerList;

    public virtual void setupObserver(Func<TType> activate)
    {
        this.activate = activate;
    }

    public virtual TType trigger(TType datum)
    {
        return activate(datum);
    }
}

public class FloatAdjuster : Adjuster<float>
{
    private List<FloatAdjuster> observerList;

    public override void setupObserver(Func<float> activate)
    {
        this.activate = activate;
    }

    public override float trigger(float datum)
    {
        return activate(datum);
    }

    public void connect(List<FloatAdjuster> observers)
    {
        this.observerList = observers;
    }

    public void complete()
    {
        if (observerList.Contains(this))
            observerList.Remove(this);
        Destroy(this);
    }
}