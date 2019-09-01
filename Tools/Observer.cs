using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer<TType>: MonoBehaviour
{
    public delegate void Func<TType1>(TType1 datum);
    protected Func<TType> activate;
    private List<Observer<TType>> observerList;

    public virtual void setupObserver(Func<TType> activate)
    {
        this.activate = activate;
    }

    public virtual void trigger(TType datum)
    {
        activate(datum);
    }
}

public class FloatObserver : Observer<float>
{
    private List<FloatObserver> observerList;

    public override void setupObserver(Func<float> activate)
    {
        this.activate = activate;
    }

    public override void trigger(float datum)
    {
        activate(datum);
    }

    public void connect(List<FloatObserver> observers)
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

public class IntObserver : Observer<int>
{
    private List<IntObserver> observerList;

    public override void setupObserver(Func<int> activate)
    {
        this.activate = activate;
    }

    public override void trigger(int datum)
    {
        activate(datum);
    }

    public void connect(List<IntObserver> observers)
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

public class GameObjectObserver : Observer<GameObject>
{
    private List<GameObjectObserver> observerList;

    public override void setupObserver(Func<GameObject> activate)
    {
        this.activate = activate;
    }

    public override void trigger(GameObject datum)
    {
        activate(datum);
    }

    public void connect(List<GameObjectObserver> observers)
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

public class comboObserver : Observer<(ObjectInteractable, BaseSkill)>
{
    private List<comboObserver> observerList;

    public override void setupObserver(Func<(ObjectInteractable, BaseSkill)> activate)
    {
        this.activate = activate;
    }

    public override void trigger((ObjectInteractable, BaseSkill) datum)
    {
        activate(datum);
    }

    public void connect(List<comboObserver> observers)
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

public class effectObserver : Observer<IEffect>
{
    private List<effectObserver> observerList;

    public override void setupObserver(Func<IEffect> activate)
    {
        this.activate = activate;
    }

    public override void trigger(IEffect datum)
    {
        activate(datum);
    }

    public void connect(List<effectObserver> observers)
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
/*
 * unsure if I want to implement this.
public class ActorObserver : Observer<ObjectActor>
{
    private List<ActorObserver> observerList;

    public override void setupObserver(Func<ObjectActor> activate)
    {
        this.activate = activate;
    }

    public override void trigger(ObjectActor datum)
    {
        activate(datum);
    }

    public void connect(List<ActorObserver> observers)
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
*/
